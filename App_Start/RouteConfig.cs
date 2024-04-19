﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Property_Rental_Management
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Properties",
                url: "Properties/Index",
                defaults: new { controller = "Properties", action = "Index" }
             );
            routes.MapRoute(
                name: "SendMessage",
                url: "Messages/SendMessage",
                defaults: new { controller = "Messages", action = "SendMessage" }
             );





        }
    }
}