using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

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