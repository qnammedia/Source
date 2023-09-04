using DotNetNuke.Web.Api;
using System.Web.Http;

namespace QNMedia.CMSAPI
{
    public class RouterMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute(
            "Services",
            "default",
            "{controller}/{action}",
            new string[] { "QNMedia.CMSAPI" });
        }
    }
}