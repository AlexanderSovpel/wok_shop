using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WokShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WokShop.Controllers
{
    public class AccountController : Controller
    {
        FoodContext db = new FoodContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string confirmPassword)
        {
            if (!password.Equals(confirmPassword))
                return Content("Введённые пароли не совпадают");

            PasswordHasher ph = new PasswordHasher();
            string hashPassword = ph.HashPassword(password);

            User newUser = new User
            {
                Email = email,
                Password = hashPassword,
                Role = "admin"
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return Redirect("/Account/Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            PasswordHasher ph = new PasswordHasher();
            string hashPassword = ph.HashPassword(password);
            PasswordVerificationResult pr = ph.VerifyHashedPassword(hashPassword, password);

            User user = db.Users.Where(u => u.Email.Equals(email)).First();
            if (user != null)
            {
                if (pr == PasswordVerificationResult.Success)
                {
                    Session["user"] = user;
                    return Redirect("/Foods/Foods");
                }
                else
                    return Content("Неверный пароль");
            }
            else
                return Content("Пользователь не найден");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return Redirect("/Home/Index");
        }
    }
}