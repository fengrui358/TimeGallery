using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TimeGallery.Models;

namespace TimeGallery.Interfaces
{
    public interface ISessionManager : IDependency
    {
        bool VerifySession(HttpContextBase httpContext);

        void AddSession(string openId);
    }
}
