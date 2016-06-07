using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Managers
{
    public class ContentManager : IContentManager
    {
        private IUserManager _userManager;
        private IGalleryManager _galleryManager;

        public ContentManager(IUserManager userManager, IGalleryManager galleryManager)
        {
            _userManager = userManager;
            _galleryManager = galleryManager;
        }

        public bool AddContent(string openId, long galleryId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                throw new ArgumentNullException(nameof(openId));
            }

            var userModel = _userManager.GetUser(openId);
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            if (galleryId <= 0)
            {
                throw new ArgumentNullException(nameof(galleryId));
            }

            //todo：增加内容
            return true;
        }
    }
}