using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using CouchDBSample.Models;
using RedBranch.Hammock;

namespace CouchDBSample.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var repository = GetRepository<Person>();

			return View(repository.All());
		}

		public ActionResult Create(Person person)
		{
			var repository = GetRepository<Person>();
			repository.Save(person);

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
