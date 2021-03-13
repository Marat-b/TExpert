using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using TExp.Models;


namespace TExp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional } 
            );
            config.MapODataServiceRoute("ODataRoute", "odata", GetModel());
        }

        public static IEdmModel GetModel()
        {
            ODataModelBuilder builder=new ODataConventionModelBuilder();
            builder.EntitySet<SearchViewModel>("Equipments"); //.EntityType.HasKey(k=>k.EquipmentId);
            builder.EntitySet<DocumentModel>("Document");

            return builder.GetEdmModel();
        }
    }
}
