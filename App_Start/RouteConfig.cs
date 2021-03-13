﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
//using TExp.Infrastructure;

namespace TExp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Finder",
                url: "Found/Found",
                defaults: new { controller = "Found", action = "Found" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            #region Web API Routes
            
            // Web API Session Enabled Route Configurations
            /*routes.MapHttpRoute(
                name: "SessionsRoute",
                routeTemplate: "api/sessions/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionEnabledHttpControllerRouteHandler();*/

            /*
            routes.MapHttpRoute(
                name: "SessionsRoute2",
                routeTemplate: "api/sessions/{controller}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionEnabledHttpControllerRouteHandler();*/

            // Web API Stateless Route Configurations
            /*routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/
            #endregion
        }
    }
}
