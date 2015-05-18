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

        /// <summary>
        /// Извлекает все жанры из бд
        /// </summary>
        /// <returns>Страница со всеми жанрами</returns>
        [Authorize(Roles = "Администратор")]
        public ActionResult Index()
        {
            var genres = db.genre.ToList();
            return View(genres);
        }

        
        /// <summary>
        /// Создание жанра
        /// </summary>
        /// <param name="genreName">Наименование жанра</param>
        /// <returns>Страница со всеми жанрами</returns>
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

        /// <summary>
        /// Выборка книг одного жанра
        /// </summary>
        /// <param name="id">Id жанра в бд</param>
        /// <returns>Сраница книг жанра</returns>
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

        /// <summary>
        /// Удаление жанра
        /// </summary>
        /// <param name="id">Id жанра в бд</param>
        /// <returns>Частичное представление для подтверждения</returns>
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

        /// <summary>
        /// Подтверждение удаления жанра
        /// </summary>
        /// <param name="id">Id жанра в бд</param>
        /// <returns>Страница всех жанров</returns>
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
