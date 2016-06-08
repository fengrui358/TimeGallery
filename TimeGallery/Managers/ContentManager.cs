using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeGallery.Consts;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Managers
{
    public class ContentManager : IContentManager
    {
        private readonly IUserManager _userManager;
        private IGalleryManager _galleryManager;

        public ContentManager(IUserManager userManager, IGalleryManager galleryManager)
        {
            _userManager = userManager;
            _galleryManager = galleryManager;
        }

        public bool AddContent(string openId, ref ContentModel contentModel, out string errorMsg)
        {
            errorMsg = ErrorString.SystemInnerError;

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


            errorMsg = string.Empty;
            return true;
        }
    }
}