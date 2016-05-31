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
using NLog;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using TimeGallery.DataBase;
using TimeGallery.DataBase.Entity;
using TimeGallery.Filters;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using TimeGallery.Weixin;

namespace TimeGallery.Controllers
{
    [AuthFilter]
    public class GalleryController : ControllerBase
    {
        public GalleryController(IConfigurationManager configurationManager, IUserManager userManager,
            ISessionManager sessionManager, IGalleryManager galleryManager) : base(configurationManager, userManager, sessionManager, galleryManager)
        {
        }

        // GET: Gallery
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = ConfigurationManager.WebTitle;

            IEnumerable<ContentDbEntity> contents = new List<ContentDbEntity>();
            using (var con = StorageHelper.GetConnection())
            {
                contents = await con.FindAsync<ContentDbEntity>();
            }               

            var result = from content in contents
                let t = content.CreateTime
                group content by new DateTime(t.Year, t.Month, t.Day)
                into g
                select new ContentWrapperModel(g.Key, g);

            return View(result);
        }

        public ActionResult Upload()
        {
            ViewBag.UpToken = GetQiniuToken();
            return View();
        }

        //[HttpPost]
        public ActionResult GetQiniuToken()
        {
            var upToken = QiniuHelper.GetToken();

            return Json(new {uptoken = upToken}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加类容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddContent(AddContentModel addContentModel)
        {
            var result = false;
            using (var con = StorageHelper.GetConnection())
            {
                var content = (ContentDbEntity) addContentModel;
                await con.InsertAsync(content);

                if (content.Id > 0)
                {
                    result = true;
                }
            }

            return Json(new {success = result });
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Register()
        {
            var userModel = GetUser();
            
            //判断是否存在已注册的相册

            return Content("注册相册");
        }

        public ActionResult Manager()
        {
            return Content("管理相册");
        }

        public ActionResult Follow()
        {
            return Content("关注相册");
        }

        public ActionResult Invite()
        {
            return Content("邀请关注");
        }
    }
}