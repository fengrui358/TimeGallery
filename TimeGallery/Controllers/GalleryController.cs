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
using TimeGallery.Filters;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using TimeGallery.Weixin;

namespace TimeGallery.Controllers
{
    [AuthFilter]
    public class GalleryController : Controller
    {
        private IConfigurationManager _configurationManager;
        private ISessionManager _sessionManager;

        public GalleryController(IConfigurationManager configurationManager, ISessionManager sessionManager)
        {
            _configurationManager = configurationManager;
            _sessionManager = sessionManager;
        }

        // GET: Gallery
        public ActionResult Index()
        {
            ViewBag.Title = _configurationManager.GetAppSetting("WebTitle");
            var contents = StorageHelper.Search(DateTime.Now);

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
        public ActionResult AddContent(AddContentModel addContentModel)
        {
            var result = StorageHelper.InsertContent(addContentModel);

            return Json(new {success = result});
        }        
    }
}