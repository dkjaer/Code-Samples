using System.Web.Mvc;
using BusinessLogic;
using Common;

namespace RestfulApi.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
		{
			ViewBag.Title = "Test Home";
			var users = UserBusinessLogic.GetUsers();
			return View(users);
		}

		// GET: User/5
		[HttpGet]
		[Route("User/{id}")]
		public ActionResult Details(int id)
		{
			var user = UserBusinessLogic.GetUser(id);
			return View("User", user);
		}

		// POST: User
		[HttpPost]
		[ActionName("User")]
		public ActionResult Create(User user)
		{
			user = UserBusinessLogic.AddUpdateUser(user);
			return RedirectToAction("");
		}

        // PUT: User/Edit/5
		[HttpPut]
		[ActionName("User")]
		[Route("/User/{id}")]
		public ActionResult Edit(int id, User user)
		{
			user = UserBusinessLogic.AddUpdateUser(user);
			return RedirectToAction("");
		}

		// DELETE: User/5
		[HttpDelete]
		[ActionName("User")]
		[Route("/User/{id}")]
		public ActionResult Delete(int id)
		{
			UserBusinessLogic.DeleteUser(id);
			return RedirectToAction("");
		}
    }
}
