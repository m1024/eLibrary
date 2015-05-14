using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;

namespace HelpDeskTrain.Controllers
{
    [Authorize(Roles = "Администратор, Пользователь")]
    public class UserController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        [HttpGet]
        public ActionResult Index()
        {
            var users = db.user.Include(u => u.Role).ToList();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Create()
        {
            SelectList roles = new SelectList(db.role, "Id", "Name");
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.user.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList roles = new SelectList(db.role, "Id", "Name");
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Edit(int id)
        {
            User user = db.user.Find(id);
            SelectList roles = new SelectList(db.role, "Id", "Name", user.RoleId);
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList roles = new SelectList(db.role, "Id", "Name");
            ViewBag.Roles = roles;

            return View(user);
        }


        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            User b = db.user.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return PartialView(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Администратор")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            User b = db.user.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.user.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateUser()
        {
            SelectList roles = new SelectList(db.role, "Id", "Name");
            ViewBag.Roles = roles;
            return View("CreateUser");
        } 

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateUser(User user)
        {
            	
            User user1 = db.user.FirstOrDefault(u => u.Login == user.Login);
            if (user1 == null)
            {
                user.RoleId = 2;
                if (ModelState.IsValid)
                {
                    db.user.Add(user);
                    db.SaveChanges();
                    ViewBag.Message = "Вы успешно зарегистрировались";
                    return RedirectToAction("Index","Home");
                }

                SelectList roles = new SelectList(db.role, "Id", "Name");
                ViewBag.Roles = roles;
            }   
            else ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
            return View("CreateUser");
        }
    }
}