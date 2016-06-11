using System.Web;
using System.Web.Mvc;
using TimeGallery.Filters;

namespace TimeGallery
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TrackPageLoadPerformanceAttribute());
        }
    }
}
