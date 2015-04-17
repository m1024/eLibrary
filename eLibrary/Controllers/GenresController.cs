using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class GenresController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var genres = db.genre.ToList();
            return View(genres);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public ActionResult Create(string genreName)
        {
            if (genreName != "")
            {
                Genre genre = new Genre();
                genre.Name = genreName;
                db.genre.Add(genre);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Genre g = db.genre.Find(id);
            db.genre.Remove(g);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
