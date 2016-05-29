using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using TimeGallery.Controllers;
using TimeGallery.Helper;
using TimeGallery.Interfaces;

namespace TimeGallery.Weixin
{
    public class WeixinManager : IWeixinManager
    {
        private IConfigurationManager _configurationManager;

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

        public WeixinManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void Init()
        {
            AccessTokenContainer.Register(AppId, WeixinAppSecret);

            InitDefaultMenus();
        }

        /// <summary>
        /// 初始化用户的默认菜单
        /// </summary>
        private void InitDefaultMenus()
        {
            var buttonGroup = new ButtonGroup();

            #region  第一个菜单

            var timeGalleryBtn = new SingleViewButton
            {
                name = _configurationManager.WebTitle,
                url = _configurationManager.WebName
            };

            buttonGroup.button.Add(timeGalleryBtn);
            
            #endregion

            #region 第三个菜单

            var aboutBtn = new SubButton("关于");

            var aboutSubBtn = new SingleViewButton
            {
                name = "关于",
                url = SimpleUrlHelper.GenerateUrl(nameof(GalleryController), nameof(GalleryController.About))
            };

            var registerBtn = new SingleViewButton
            {
                name = "注册相册",
                url = SimpleUrlHelper.GenerateUrl(nameof(GalleryController), nameof(GalleryController.Register))
            };

            var followBtn = new SingleViewButton
            {
                name = "关注相册",
                url = SimpleUrlHelper.GenerateUrl(nameof(GalleryController), nameof(GalleryController.Follow))
            };

            var inviteBtn = new SingleViewButton
            {
                name = "邀请关注",
                url = SimpleUrlHelper.GenerateUrl(nameof(GalleryController), nameof(GalleryController.Invite))
            };

            aboutBtn.sub_button.Add(aboutSubBtn);
            aboutBtn.sub_button.Add(registerBtn);
            aboutBtn.sub_button.Add(followBtn);
            aboutBtn.sub_button.Add(inviteBtn);

            buttonGroup.button.Add(aboutBtn);

            #endregion

            var result = CommonApi.CreateMenu(AppId, buttonGroup);

            if (result == null || result.errcode != 0)
            {
                throw new Exception("初始化微信默认菜单出错!");
            }
        }        
    }
}