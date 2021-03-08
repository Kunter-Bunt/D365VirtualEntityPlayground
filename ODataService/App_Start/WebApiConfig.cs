using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using ODataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ODataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var builder = new ODataConventionModelBuilder();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Item>("Items");

            config.MapODataServiceRoute("ODataRoute", null, builder.GetEdmModel());

            // Web API routes
            /*
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            */

        }
    }
}
