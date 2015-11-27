using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Board",
                url: "Forum/Board/{boardId}",
                defaults: new { controller = "Forum", action = "Board", boardId = "b" }
            );

            routes.MapRoute(
                name: "Thread",
                url: "Forum/Thread/{threadId}",
                defaults: new { controller = "Forum", action = "Thread", threadId = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
