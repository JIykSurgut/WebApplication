using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Models;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }
        public AccountController(UserManager<User, int> userManager, SignInManager<User, int> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        private UserManager<User, int> userManager;
        private SignInManager<User, int> signInManager;
        public UserManager<User, int> UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>();
            }
            private set
            {
                userManager = value;                
            }
        }
        public SignInManager<User, int> SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<SignInManager<User, int>>();
            }
            private set
            {
                signInManager = value;
            }
        }

        #region /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success: return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut: return View("Lockout");
                case SignInStatus.RequiresVerification: return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }
        }
        #endregion

        #region /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Отправка сообщения электронной почты с этой ссылкой
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Подтверждение учетной записи", "Подтвердите вашу учетную запись, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }
        #endregion


        //POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET:
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Test()
        {
            int userId = 6;
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action(
               "ConfirmEmail", "Account",
               new { userId = userId, code = code },
               protocol: Request.Url.Scheme);

            await UserManager.SendEmailAsync(userId,
               "Confirm your account",
               "Please confirm your account by clicking this link: <a href=\""
                                               + callbackUrl + "\">link</a>");
            return View();
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        //Возвращает пользователя обратно на форму после аутентификации
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

    }
}