using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Elegant.Models;

namespace Elegant.Controllers
{
    public class AccountController : Controller
    {
        // Єдина адреса, яка отримує права адміністратора
        private const string AdminEmail = "elegant@gmail.com";

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Shop");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            if (user == null || !Crypto.VerifyHashedPassword(user.PasswordHash, model.Password))
            {
                ModelState.AddModelError("", "Невірний email або пароль.");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Shop");
        }

        // GET: /Account/Register
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Shop");
            }
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = db.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
            if (existing != null)
            {
                ModelState.AddModelError("Email", "Користувач з такою електронною адресою вже зареєстрований.");
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                FullName = model.FullName,
                // Пароль ніколи не зберігається у відкритому вигляді — лише PBKDF2-хеш
                PasswordHash = Crypto.HashPassword(model.Password),
                CreatedAt = DateTime.Now,
                IsAdmin = string.Equals(model.Email, AdminEmail, StringComparison.OrdinalIgnoreCase)
            };

            db.Users.Add(user);
            db.SaveChanges();

            FormsAuthentication.SetAuthCookie(user.Email, false);

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Shop");
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
