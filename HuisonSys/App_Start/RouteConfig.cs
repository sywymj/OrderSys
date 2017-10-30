using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HuisonSys
{
    public class RouteConfig
    {

        // 需要主要添加路由的时候，必须添加 namespaces规则
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{area}/{controller}/{action}/{id}",
                defaults: new { area="Admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "OrderSys.Admin.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Default_area",
            //    url: "{area}/{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", area = "Weixin",id = UrlParameter.Optional },
            //    namespaces: new string[] { "HuisonSys.Controllers" }
            //);
        }
    }
}