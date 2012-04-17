using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using HammockTest.Models;
using RedBranch.Hammock;

namespace HammockTest.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var repository = GetRepository<Thingy>();

			return View(repository.All());
		}

		public ActionResult Create(Thingy thingy)
		{
			var repository = GetRepository<Thingy>();
			repository.Save(thingy);

			return RedirectToAction("Index");
		}

		private Repository<T> GetRepository<T>() where T : class
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
