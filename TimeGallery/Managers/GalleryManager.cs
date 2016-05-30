using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeGallery.Interfaces;

namespace TimeGallery.Managers
{
    public class GalleryManager : IGalleryManager
    {
        private IUserManager _userManager;

        public GalleryManager(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}