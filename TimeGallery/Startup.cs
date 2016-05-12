using System;
using Microsoft.Owin;
using NLog;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimeGallery.Startup))]
namespace TimeGallery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
