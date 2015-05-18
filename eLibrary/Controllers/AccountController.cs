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
        /// <summary>
        /// Возвращает форму авторизации
        /// </summary>
        /// <returns>Форма авторизации</returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Обработка формы авторизации
        /// </summary>
        /// <param name="model">Введенные пользователем авторизационные данные</param>
        /// <param name="returnUrl">url адрес</param>
        /// <returns>Перенаправление на главную или на форму авторизации</returns>
        [HttpPost]
        public ActionResult Login(LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы
        /// </summary>
        /// <returns>Перенаправление на главную</returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Валидация пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>true - валидация пройдена, false - нет</returns>
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
