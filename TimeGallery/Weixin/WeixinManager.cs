using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;

namespace TimeGallery.Weixin
{
    public class WeixinManager
    {
#if DEBUG
        public static readonly string Token = "0B323312CC614037932DBF88B4739105";//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = "";//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string AppId = "wxc78079d70e940ab4";//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string WeixinAppSecret = "d4624c36b6795d1d99dcf0547af5443d";//与微信公众账号后台的AppId设置保持一致，区分大小写。
#else
        public static readonly string Token = WebConfigurationManager.AppSettings["WeixinToken"];//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string WeixinAppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];//与微信公众账号后台的AppId设置保持一致，区分大小写。
#endif

        public static void Init()
        {
            AccessTokenContainer.Register(AppId, WeixinAppSecret);
            //初始化用户的默认菜单
            InitMenus();
        }

        private static void InitMenus()
        {
            var buttonGroup = new ButtonGroup();
            //第一个菜单
            var timeGalleryBtn = new SubButton("时光轴");

            var pepeWebBtn = new SingleViewButton
            {
                name = "裴裴时光轴",
                url = "http://fengpepe.com"
            };

            var aboutBtn = new SingleClickButton
            {
                name = "关于",
                key = "about"
            };

            timeGalleryBtn.sub_button.Add(pepeWebBtn);
            buttonGroup.button.Add(timeGalleryBtn);
            buttonGroup.button.Add(aboutBtn);

            var result = CommonApi.CreateMenu(AppId, buttonGroup);
        }
    }
}