using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class SeriesController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Main()
        {
            var series = db.serie.ToList();
            return View(series);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create(Series serie)
        {
            if (serie.Name != "")
            {
                db.serie.Add(serie);
                db.SaveChanges();
            }
            return RedirectToAction("Main");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Edit(int id = 0)
        {
            Series serie = db.serie.Find(id);
            if (serie == null)
            {
                return HttpNotFound();
            }
            return PartialView(serie);
        }


        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Edit(Series serie)
        {
            if (serie == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Entry(serie).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Main");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Series s = db.serie.Find(id);
            db.serie.Remove(s);
            db.SaveChanges();

            return RedirectToAction("Main");
        }

        public ActionResult SerieSearch(string serieName)
        {
            IEnumerable<Series> findSerie = null;
            if (serieName != "")
            {
                if (serieName == "all")
                {
                    findSerie = db.serie.ToList();
                }
                else
                findSerie = from series in db.serie
                            where series.Name.Contains(serieName)
                            select series;
            }
            if (findSerie == null)
            {
                return HttpNotFound();
            }
            return PartialView(findSerie.ToList());
        }

        [AllowAnonymous]
        public ActionResult ShowSerie(int? id)
        {
            if (id != null)
            {
                Series serie = db.serie.Find(id);
                if (serie == null) return HttpNotFound();

                return View(serie);
            }
            else
                return View();
        }

        [AllowAnonymous]
        public ActionResult BooksBySerie(int id)
        {
            IEnumerable<Book> findBooks = from books in db.book
                                          where books.SerieId == id
                                          select books;
            return PartialView(findBooks.ToList());
        }
    }
}
