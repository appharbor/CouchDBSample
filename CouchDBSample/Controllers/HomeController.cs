using System;
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
		private readonly IRepository<Person> repository = MvcApplication.GetRepository<Person>();

		public ActionResult Index()
		{
			return View(repository.Where(x => x.Age).Ge(50));
		}

		public ActionResult Create()
		{
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
