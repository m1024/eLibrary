using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class SeriesController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Index()
        {
            var series = db.serie.ToList();
            return View(series);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create(string serieName, string serieDescription)
        {
            if (serieName != "")
            {
                Series serie = new Series();
                serie.Name = serieName;
                serie.Description = serieDescription;
                db.serie.Add(serie);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        public ActionResult SerieSearch(string serieName)
        {
            IEnumerable<Series> findSerie = null;
            if (serieName != null)
            {
                findSerie = from series in db.serie
                            where series.Name == serieName
                            select series;
            }
            if (findSerie == null)
            {
                return HttpNotFound();
            }
            return PartialView(findSerie.ToList());
        }
    }
}
