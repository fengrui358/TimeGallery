using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Serialization;
using Autofac;
using Dapper.FastCrud;
using Newtonsoft.Json;
using NLog;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using TimeGallery.Consts;
using TimeGallery.DataBase;
using TimeGallery.DataBase.Entity;
using TimeGallery.Enums;
using TimeGallery.Filters;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using TimeGallery.Models.Javascript;
using TimeGallery.Weixin;

namespace TimeGallery.Controllers
{
    [AuthFilter]
    public class GalleryController : ControllerBase
    {
        public GalleryController(IConfigurationManager configurationManager, IUserManager userManager,
            ISessionManager sessionManager, IGalleryManager galleryManager, IContentManager contentManager)
            : base(configurationManager, userManager, sessionManager, galleryManager, contentManager)
        {
        }

        #region 展示相册

        // GET: Gallery
        public ActionResult Index(long id)
        {
            //ViewBag.Title = ConfigurationManager.WebTitle;

            //IEnumerable<ContentDbEntity> contents = new List<ContentDbEntity>();
            //using (var con = StorageHelper.GetConnection())
            //{
            //    contents = await con.FindAsync<ContentDbEntity>();
            //}               

            //var result = from content in contents
            //    let t = content.CreateTime
            //    group content by new DateTime(t.Year, t.Month, t.Day)
            //    into g
            //    select new ContentWrapperModel(g.Key, g);

            //return View(result);

            //如果没有传入相册的id值则默认选择优先级最高的相册
            ViewBag.GalleryId = id;
            return View();
        }

        /// <summary>
        /// 获取相册数据的核心方法
        /// </summary>
        /// <param name="id">相册id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGalleryContents(long id)
        {
            
            return Content("");
        }

        #endregion

        #region 上传内容

        //[AuthFilter(false)]
        //public ActionResult Index(long galleryId)
        //{
        //    return null;
        //}

        public async Task<ActionResult> Upload()
        {
            return await Task.Run(() =>
            {
                ViewBag.UpToken = GetQiniuToken();

                var gallery = GalleryManager.GetGalleryModelCanUpdate(CurrentUserModel);
                if (gallery == null)
                {
                    //todo：设计一个友好的提示过度页面，不然不知道为什么就跳转到这个页面来了
                    //没有找到可管理的相册，跳转到关注页面
                    return RedirectToAction(nameof(ShowFollowGalleryList)) as ActionResult;
                }

                return View(gallery);
            }).ContinueWith(task => task.Result);
        }

        /// <summary>
        /// 添加类容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddContent(ContentModel addContentModel)
        {
            if (addContentModel == null)
            {
                var result = new RequestResult(RequestResultTypeDefine.Error, ErrorString.SystemInnerError);

                return Content(JsonConvert.SerializeObject(result));
            }

            return await Task.Run(() =>
            {
                string errorMsg;
                RequestResult result = null;

                if (ContentManager.AddContent(CurrentUserModel, ref addContentModel, out errorMsg))
                {
                    result = new RequestResult(RequestResultTypeDefine.Success);
                }
                else
                {
                    result = new RequestResult(RequestResultTypeDefine.Error, errorMsg);
                }

                return Content(JsonConvert.SerializeObject(result));
            }).ContinueWith(task => task.Result);
        }

        #endregion

        #region 关于

        public ActionResult About()
        {            
            return View();
        }

        #endregion

        #region 注册相册

        public ActionResult Register()
        {
            //判断是否存在已注册的相册
            var gallery = GalleryManager.GetGalleryModels(CurrentUserModel.OpenId, UserGalleryRelTypeDefine.Owner);
            if (gallery.Any())
            {
                //相册已存在，导航到管理相册页面
                return RedirectToAction(nameof(Manager));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult RegisterSubmit(GalleryModel galleryModel)
        {
            if (galleryModel == null)
            {
                throw new ArgumentNullException(nameof(galleryModel));
            }

            //校验相册基本信息
            if (string.IsNullOrEmpty(galleryModel.Name))
            {
                LogManager.GetCurrentClassLogger().Error("校验相册基本信息不通过");
                var result = new RequestResult(RequestResultTypeDefine.Error, "相册名不能为空");

                return Content(JsonConvert.SerializeObject(result));
            }
            else
            {
                string errorMsg;
                if (GalleryManager.RegisterGalleryModel(CurrentUserModel, ref galleryModel, out errorMsg))
                {
                    var result = new RequestResult(RequestResultTypeDefine.Success, "点击确定可立即开始上传文件");
                    return Content(JsonConvert.SerializeObject(result));
                }
                else
                {
                    var result = new RequestResult(RequestResultTypeDefine.Error, errorMsg);
                    return Content(JsonConvert.SerializeObject(result));
                }
            }
        }

        #endregion


        #region 管理相册

        public ActionResult Manager()
        {
            return Content("管理相册");
        }

        #endregion


        #region 关注相册

        /// <summary>
        /// 展示可关注的相册列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowFollowGalleryList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchGalleryList(string searchKey)
        {
            var galleryModels = GalleryManager.SearchAllGalleryModels(searchKey);
            var result = new RequestResult<IEnumerable<GalleryModel>>(RequestResultTypeDefine.Success)
            {
                Result = galleryModels
            };
            return Content(JsonConvert.SerializeObject(result));
        }

        #endregion


        #region 邀请关注

        public ActionResult Invite()
        {
            return Content("邀请关注");
        }

        #endregion


        #region 云存储

        //[HttpPost]
        public ActionResult GetQiniuToken()
        {
            var upToken = QiniuHelper.GetToken();

            return Json(new {uptoken = upToken}, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}