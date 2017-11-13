using System.Web.Mvc;

namespace RestfulApi.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Code Samples";

			return View();
		}
	}
}
