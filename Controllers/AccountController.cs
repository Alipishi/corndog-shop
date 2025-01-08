using System.Security.Claims;
using HelmonyCornDog.Data.Repositories;
using HelmonyCornDog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HelmonyCornDog.Controllers
{
    public class AccountController : Controller
    {

        #region Injection

        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion



        #region Register Get&Post

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }


            Users user = new Users()
            {
                Name = register.Name,
                Email = register.Email.ToLower(),
                Password = register.Password,
                IsAdmin = false,
                RegisterDate = DateTime.Now
            };
            _userRepository.AddUser(user);


            //Passing model data to show the modal in Home index
            TempData["Name"]= register.Name;

            return RedirectToAction("Index", "Home", new { showModal = true });
        }

        public IActionResult VerifyEmail(string email)
        {
            if (_userRepository.IsExistUserByEmail(email.ToLower()))
            {
                return Json($"ایمیل {email} در سیستم موجود است لطفا به صفحه ورود مراجعه کنید.");
            }

            return Json(true);
        }

        #endregion



        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = _userRepository.GetUserForLogin(login.Email.ToLower(),login.Password);

            if (user == null)
            {
                ModelState.AddModelError("Email","اطلاعات وارد شده صحیح نیست!");
                return View(login);
            }

            //login cookie

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("IsAdmin", user.IsAdmin.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RemmemberMe
            };
            HttpContext.SignInAsync(principal, properties);

            return Redirect("/");
        }

        #endregion

        #region LogOut

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Login");
        }

        #endregion
    }
}
