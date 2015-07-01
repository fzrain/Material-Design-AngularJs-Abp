using System;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using Fzrain.Users;

namespace Fzrain.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FzrainControllerBase
    {
       

        public ActionResult Index()
        {          
            return View("~/Views/index.cshtml"); //Layout of the angular application.
        }
	}
}