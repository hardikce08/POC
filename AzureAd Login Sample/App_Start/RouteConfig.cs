using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace POC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Dashboard",
              url: "Dashboard",
              defaults: new { controller = "Home", action = "Dashboard", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "ReportNew",
              url: "ReportNew",
              defaults: new { controller = "Home", action = "HighChart", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Contract",
              url: "Contract",
              defaults: new { controller = "Contract", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Product",
              url: "Product",
              defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "TransferRequest",
             url: "TransferRequest",
             defaults: new { controller = "TransferRequest", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
           name: "Event",
           url: "Event",
           defaults: new { controller = "Event", action = "Index", id = UrlParameter.Optional }
       );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Dashboard", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "GetTimeLineGraph",
            //    url: "Report/TestLineData",
            //    defaults: new { controller = "Report", action = "TestLineData" }
            //);
        }
    }
}
