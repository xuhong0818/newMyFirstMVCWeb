using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyFirstMVCWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                //規定路由規則,id為頁面傳遞參數
                //url:"{home}/{imdex}/{id}"
                url: "{controller}/{action}/{id}",
                //起始訪問網站莫認為home/index
                //調用控制器裡的home,方法為index
                defaults: new { controller = "one", action = "register", id = UrlParameter.Optional }
                );


        }
    }
}
