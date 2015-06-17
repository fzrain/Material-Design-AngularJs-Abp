using System;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Fzrain.Users;

namespace Fzrain.Web.Controllers
{
    public class HomeController : FzrainControllerBase
    {
        private IRepository<User,Guid> userRepository;

        public HomeController(IRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        public ActionResult Index()
        {
            ViewBag.User = userRepository.FirstOrDefault(u => true);
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}