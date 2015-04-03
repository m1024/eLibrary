using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                   // if (Url.IsLocalUrl(returnUrl))
                   // {
                    return RedirectToAction("Index", "Home");
                   // }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            //return System.Web.UI.WebControls.View(model);
            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            using (eLibraryContext _db = new eLibraryContext())
            {
                try
                {
                    User user = (from u in _db.user
                                 where u.Login == login && u.Password == password
                                 select u).FirstOrDefault();


                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }


    }
}
