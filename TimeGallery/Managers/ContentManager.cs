using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Dapper.FastCrud;
using NLog;
using TimeGallery.Consts;
using TimeGallery.DataBase;
using TimeGallery.Enums;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Managers
{
    public class ContentManager : IContentManager
    {
        private ConcurrentDictionary<string, ConcurrentQueue<ContentModel>> _contentCache;
        private Timer _cacheHandlerTimer;

        /// <summary>
        /// 定时处理上传数据的时间间隔
        /// </summary>
        private TimeSpan _cacheHandleInterval = TimeSpan.FromSeconds(5);

        private ManualResetEventSlim _cacheHandleSlim = new ManualResetEventSlim();

        private readonly IUserManager _userManager;
        private readonly IGalleryManager _galleryManager;

        public ContentManager(IUserManager userManager, IGalleryManager galleryManager)
        {
            _userManager = userManager;
            _galleryManager = galleryManager;

            _contentCache = new ConcurrentDictionary<string, ConcurrentQueue<ContentModel>>();
            _cacheHandlerTimer = new Timer(AddContentHandler, null, _cacheHandleInterval, Timeout.InfiniteTimeSpan);
        }

        public bool AddContent(string openId, ref ContentModel contentModel, out string errorMsg)
        {
            errorMsg = ErrorString.SystemInnerError;

            #region 校验基本信息

            if (string.IsNullOrEmpty(openId))
            {
                errorMsg = ErrorString.NotExistUser;
                throw new ArgumentNullException(nameof(openId));
            }

            var userModel = _userManager.GetUser(openId);
            if (userModel == null)
            {
                errorMsg = ErrorString.NotExistUser;
                throw new ArgumentNullException(nameof(userModel));
            }

            if (contentModel == null)
            {
                throw new ArgumentNullException(nameof(contentModel));
            }

            if (contentModel.GalleryId <= 0)
            {
                throw new ArgumentNullException(nameof(contentModel.GalleryId));
            }

            if (string.IsNullOrEmpty(contentModel.Url))
            {
                throw new ArgumentNullException(nameof(contentModel.Url));
            }

            if (contentModel.GeContentType() == ContentTypeDefine.None)
            {
                throw new ArgumentNullException(nameof(contentModel.Type));
            }

            //校验用户有权限针对该相册上传内容
            var canUpdateGallery = _galleryManager.GetGalleryModelCanUpdate(openId);
            if (canUpdateGallery == null || canUpdateGallery.Id != contentModel.GalleryId)
            {
                throw new Exception($"用户{openId}没有权限对Id为{contentModel.GalleryId}的相册上传内容的权限");
            }

            //todo:检验单个文件的大小限制


            //todo:检验单个相册的月上传容量限制

            #endregion

            #region 缓存数据

            //记下上传时间
            contentModel.CreateTime = DateTime.Now;

            _cacheHandleSlim.Wait();

            if (_contentCache.ContainsKey(openId))
            {
                _contentCache[openId].Enqueue(contentModel);
            }
            else
            {
                var newContent = contentModel;

                _contentCache.AddOrUpdate(openId,
                    new ConcurrentQueue<ContentModel>(new List<ContentModel> {newContent}),
                    (key, value) =>
                    {
                        value.Enqueue(newContent);
                        return value;
                    });
            }

            #endregion

            errorMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// 真正处理数据上传的地方
        /// </summary>
        /// <param name="state"></param>
        private void AddContentHandler(object state)
        {
            try
            {
                _cacheHandleSlim.Reset();

                var userAndContentsKeyValues = _contentCache.ToList();
                _contentCache.Clear();

                _cacheHandleSlim.Set();

                foreach (var keyValuePair in userAndContentsKeyValues)
                {
                    AddContentProcessing(keyValuePair.Key, keyValuePair.Value.ToArray());
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                _cacheHandlerTimer.Change(_cacheHandleInterval, Timeout.InfiniteTimeSpan);
            }
        }

        private void AddContentProcessing(string openId, IEnumerable<ContentModel> contentModels)
        {
            try
            {
                //判断该相册今天是否已上传内容
                //按内容的上传时间分组
                var enumerableContentModels = contentModels as ContentModel[] ?? contentModels.ToArray();
                var contentDateGroups = from cdg in enumerableContentModels
                    group cdg by cdg.CreateTime.Date
                    into g
                    select new {Date = g.Key, ContentModels = g};

                //获取相册id，一个用户同时只能拥有一个相册的上传权限
                var gallery = _galleryManager.GetGalleryModel(enumerableContentModels.First().GalleryId);

                if (gallery == null)
                {
                    throw new ArgumentNullException($"相册Id：{enumerableContentModels.First().GalleryId}不存在");
                }

                foreach (var contentDateGroup in contentDateGroups)
                {
                    //判断是否已存在对应的分组
                    using (var con = StorageHelper.GetConnection(gallery.ContentDbHost))
                    {
                        var contentGroups =
                            con.Find<ContentGroup>(
                                statement =>
                                    statement.Where(
                                        $"{nameof(ContentGroup.GalleryId):C} = @GalleryId AND {nameof(ContentGroup.Date):C} = @Date").OrderBy($"{nameof(ContentGroup.Date):C} DESC")
                                        .Top(1)
                                        .WithParameters(new {GalleryId = gallery.Id, Date = contentDateGroup.Date}));

                        var contentGroup = contentGroups.FirstOrDefault();
                        if (contentGroup == null)
                        {
                            //不存在，添加分组
                            contentGroup = new ContentGroup
                            {
                                Id = Guid.NewGuid(),
                                GalleryId = gallery.Id,
                                Date = contentDateGroup.Date,
                                CreateTime = contentDateGroup.ContentModels.Min(s => s.CreateTime)
                            };

                            con.Insert(contentGroup);
                        }

                        //todo：是否需要再次打开
                        con.Open();
                        var transaction = con.BeginTransaction();

                        try
                        {
                            foreach (var contentModel in contentDateGroup.ContentModels)
                            {
                                if (contentModel.GeContentType() == ContentTypeDefine.Image)
                                {
                                    contentGroup.ImageCount++;
                                    gallery.TotalImageCount++;
                                }
                                else if (contentModel.GeContentType() == ContentTypeDefine.Video)
                                {
                                    contentGroup.VideoCount++;
                                    gallery.TotalVideoCount++;
                                }

                                contentGroup.TotalSize += contentModel.Size;
                                gallery.TotalSize += contentModel.Size;
                                gallery.LastUpdateTime = DateTime.Now;

                                contentModel.Id = Guid.NewGuid();
                                contentModel.ContentGroupId = contentGroup.Id;

                                con.Insert(contentModel, statement => statement.AttachToTransaction(transaction));
                            }

                            con.Update(contentGroup, statement => statement.AttachToTransaction(transaction));

                            //因为gallery实在主数据库中，所以要用不同连接
                            using (var conMaster = StorageHelper.GetConnection())
                            {
                                conMaster.Update(gallery, statement => statement.AttachToTransaction(transaction));
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //隔离错误
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
    }
}