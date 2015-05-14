using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;
using System.Data.Entity;

namespace eLibrary.Controllers
{
    public class GenresController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        [Authorize(Roles = "Администратор")]
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
        [AllowAnonymous]
        public ActionResult Books(int? id)
        {
            try
            {
                IEnumerable<Book> books = null;
                books = from book in db.book.Include(u => u.Authors).Include(u => u.Serie)
                    where id == book.GenreId
                    select book;
                return View(books);
            }
            catch
            {
                ViewBag.result = "не найдено";
                return View();
            }       
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Genre b = db.genre.Find(id);
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
            Genre b = db.genre.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.genre.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
