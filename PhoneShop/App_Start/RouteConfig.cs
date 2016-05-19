using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhoneShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Login",
                url: "Auth/{action}",
                defaults: new { controller = "Authentication", action = "Login" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{TypeID}",
                defaults: new { controller = "Home", action = "Index", TypeID = UrlParameter.Optional }
            );
            
        }
    }
}
