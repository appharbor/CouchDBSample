using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using RedBranch.Hammock;
using System.Configuration;
using System;

namespace CouchDBSample
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}

		public static Repository<T> GetRepository<T>() where T : class
		{
			const string databaseName = "db";
			var connection = new Connection(
				new Uri(ConfigurationManager.AppSettings.Get("CLOUDANT_URL")));
			if (!connection.ListDatabases().Contains(databaseName))
			{
				connection.CreateDatabase(databaseName);
			}
			return new Repository<T>(connection.CreateSession(databaseName));
		}
	}
}
