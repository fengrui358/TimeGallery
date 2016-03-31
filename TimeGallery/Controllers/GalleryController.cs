using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Serialization;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using TimeGallery.Helper;
using TimeGallery.Models;

namespace TimeGallery.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            ViewBag.Title = WebConfigurationManager.AppSettings["WebTitle"];
            var contents = StorageHelper.Search(DateTime.Now);

            var result = from content in contents
                group content by content.CreateTime
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