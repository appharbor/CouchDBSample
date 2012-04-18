using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CouchDBSample.Models;
using RedBranch.Hammock;

namespace CouchDBSample.Controllers
{
	public class HomeController : Controller
	{
		private const int batchCount = 1000;
		private readonly Random random = new Random((int)DateTime.Now.Ticks);

		public ActionResult Index()
		{
			var repository = GetRepository<Person>();

			return View(repository.Where(x => x.Age).Ge(50));
		}

		public ActionResult Create()
		{
			var repository = GetRepository<Person>();

			var people = Enumerable.Range(0, batchCount).Select(x => new Person
			{
				Age = random.Next(0, 100),
				Name = RandomString(4),
			});

			foreach (var person in people)
			{
				repository.Save(person);
			}

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

		private string RandomString(int size)
		{
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}
	}
}
