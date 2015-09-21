using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.UI;
using Abp.Web.Mvc.Models;
using Fzrain.Authorization.Users;
using Fzrain.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;


namespace Fzrain.Web.Controllers
{
    public class AccountController : FzrainControllerBase
    {
        private readonly UserManager _userManager;

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public AccountController(UserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            var loginResult = await _userManager.LoginAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                loginModel.TenancyName
                );

            switch (loginResult.Result)
            {
                case LoginResultType.Success:
                    break;
                case LoginResultType.InvalidUserNameOrEmailAddress:
                case LoginResultType.InvalidPassword:
                    throw new UserFriendlyException("Invalid user name or password!");
                case LoginResultType.InvalidTenancyName:
                    throw new UserFriendlyException("No tenant with name: " + loginModel.TenancyName);
                case LoginResultType.TenantIsNotActive:
                    throw new UserFriendlyException("Tenant is not active: " + loginModel.TenancyName);
                case LoginResultType.UserIsNotActive:
                    throw new UserFriendlyException("User is not active: " + loginModel.UsernameOrEmailAddress);
                case LoginResultType.UserEmailIsNotConfirmed:
                    throw new UserFriendlyException("Your email address is not confirmed!");
                default: //Can not fall to default for now. But other result types can be added in the future and we may forget to handle it
                    throw new UserFriendlyException("Unknown problem with login: " + loginResult.Result);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = loginModel.RememberMe }, loginResult.Identity);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
        }
        [AllowAnonymous]
        [HttpGet]
        public async  Task<ActionResult> ConfirmEmail(long userId, string conformCode)
        {          
            IdentityResult result =await   _userManager.ConfirmEmailAsync(userId,conformCode);
            return View(result);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}