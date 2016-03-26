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

            return View();
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

            return Json(upToken, JsonRequestBehavior.AllowGet);
        }
    }
}