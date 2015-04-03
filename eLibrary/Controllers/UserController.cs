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


        [Authorize(Roles = "Администратор")]
        public ActionResult Delete(int id)
        {
            User user = db.user.Find(id);
            db.user.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateUser()
        {
            SelectList roles = new SelectList(db.role, "Id", "Name");
            ViewBag.Roles = roles;
            return View("Create");
        } 

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateUser(User user)
        {
            user.RoleId = 2;
            if (ModelState.IsValid)
            {
                db.user.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SelectList roles = new SelectList(db.role, "Id", "Name");
            ViewBag.Roles = roles;

            return View("Index");
        }
    }
}