﻿using System.Web.Mvc;
using System.Web.Routing;
using Domus.Web.UI.Infrastructure.DependencyInjection;
using Ninject;
using Rolstad.MVC.Errors;
using log4net.Config;

namespace Domus.Web.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleErrorAndLogAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Recipe", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );


        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
			 
            var handlers = Bootstrapper.GetKernel().Get<IKeepAliveHandler>();
        }
    }
}