using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
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
        private readonly ConcurrentDictionary<string, ConcurrentQueue<ContentModel>> _contentCache;
        private readonly Timer _cacheHandlerTimer;

        /// <summary>
        /// 定时处理上传数据的时间间隔
        /// </summary>
        private readonly TimeSpan _cacheHandleInterval = TimeSpan.FromSeconds(5);

        private readonly ManualResetEventSlim _cacheHandleSlim = new ManualResetEventSlim();

        private readonly IConfigurationManager _configurationManager;
        private readonly IUserManager _userManager;
        private readonly IGalleryManager _galleryManager;

        private int SmartGetContentGroupsLimit => int.Parse(_configurationManager.GetAppSetting("SmartGetContentGroupsLimit"));

        public ContentManager(IConfigurationManager configurationManager, IUserManager userManager, IGalleryManager galleryManager)
        {
            _configurationManager = configurationManager;
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

        public IEnumerable<ContentGroup> SmartGetContentGroups(long galleryId, DateTime afterDateTime)
        {
            var gallery = _galleryManager.GetGalleryModel(galleryId);

            if (gallery == null)
            {
                throw new ArgumentNullException(nameof(galleryId));
            }

            if (afterDateTime == DateTime.MinValue)
            {
                //加一天是因为参数是after，所以不加一天默认就不会包括当天的数据
                afterDateTime = DateTime.Now.AddDays(1);
            }

            var groupsWithOutContents = SmartGetContentGroupsWithLimit(gallery, afterDateTime);

            FillContentGroups(gallery, ref groupsWithOutContents);

            return groupsWithOutContents;
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
                    //todo:队列toarray出来是个什么顺序？？是不是按时间先后顺序排
                    AddContentProcessing(keyValuePair.Key, keyValuePair.Value.ToArray().OrderBy(s=>s.CreateTime));
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

        /// <summary>
        /// 线程真正上传的地方
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="contentModels"></param>
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

                        IDbTransaction transaction = null;

                        try
                        {
                            con.Open();
                            transaction = con.BeginTransaction();

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
                            transaction?.Rollback();
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

        /// <summary>
        /// 根据指定容量的限制，递归
        /// </summary>
        /// <param name="gallery">相册</param>
        /// <param name="afterDateTime">获取指定时间之后</param>
        /// <param name="totalCount">当前已获取的文件的数量</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页限制，这个处于内网获取和计算，这个值可以给大一些，减少数据库访问次数</param>
        /// <returns></returns>
        private IEnumerable<ContentGroup> SmartGetContentGroupsWithLimit(GalleryModel gallery, DateTime afterDateTime,
            int totalCount = 0, int pageIndex = 0, int pageSize = 1000)
        {
            //如果pagesize大于SmartGetContentGroupsLimit，则是一种性能浪费，浪费可耻
            pageSize = pageSize > SmartGetContentGroupsLimit ? SmartGetContentGroupsLimit : pageSize;

            var result = new List<ContentGroup>();
            var groupResult = new List<ContentGroup>();

            using (var con = StorageHelper.GetConnection(gallery))
            {
                groupResult = con.Find<ContentGroup>(
                    statement =>
                        statement.Where(
                            $"{nameof(ContentGroup.GalleryId):C} = @GallerId AND {nameof(ContentGroup.Date):C} < @AfterDateTime")
                            .OrderBy($"{nameof(ContentGroup.Date):C} DESC")
                            .Skip(pageIndex*pageSize)
                            .Top(pageSize)
                            .WithParameters(new {GallerId = gallery.Id, AfterDateTime = afterDateTime})).ToList();
            }            

            for (int i = 0; i < groupResult.Count(); i++)
            {
                result.Add(groupResult[i]);
                totalCount += groupResult[i].ImageCount + groupResult[i].VideoCount;

                if (totalCount >= SmartGetContentGroupsLimit)
                {
                    return result;
                }
            }

            if (groupResult.Count < pageSize)
            {
                //证明数据库中已经没有了，也要提前返回
                return result;
            }
            else
            {
                //一组循环完毕依然数量没有取够并且数据库中还有，则继续递归
                pageIndex++;

                result.AddRange(SmartGetContentGroupsWithLimit(gallery, afterDateTime, totalCount, pageIndex, pageSize));
                return result;
            }
        }

        /// <summary>
        /// 填充指定分组的内容
        /// </summary>
        /// <param name="gallery"></param>
        /// <param name="contentGroups"></param>
        private void FillContentGroups(GalleryModel gallery, ref IEnumerable<ContentGroup> contentGroups)
        {
            if (contentGroups.Any())
            {
                var contentGroupIds = contentGroups.Select(s => s.Id);
                var contentModels = new List<ContentModel>();

                using (var con = StorageHelper.GetConnection(gallery))
                {
                    contentModels =
                        con.Find<ContentModel>(
                            statement =>
                                statement.Where($"{nameof(ContentModel.ContentGroupId):C} IN @ContentGroupIds")
                                    .WithParameters(new {ContentGroupIds = contentGroupIds})).ToList();
                }

                foreach (var contentGroup in contentGroups)
                {
                    contentGroup.ContentModels =
                        contentModels.Where(s => s.ContentGroupId == contentGroup.Id)
                            .OrderByDescending(s => s.CreateTime).ToList();

                    contentModels.RemoveAll(s => s.ContentGroupId == contentGroup.Id);
                }
            }
        }
    }
}