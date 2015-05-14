﻿using System;
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

        [AllowAnonymous]
        public ActionResult Index()
        {
            var series = db.serie.ToList();
            return View(series);
        }


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
            if (ModelState.IsValid)
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
            int serieId = serie.Id;
            if (serie == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Entry(serie).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ShowSerie", new { id = serieId});
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
            if (s == null)
            {
                return HttpNotFound();
            }
            return PartialView(s);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Series b = db.serie.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.serie.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }







        public ActionResult SerieSearch(string serieName)
        {
            IEnumerable<Series> findSerie = null;
            //if (serieName != "")
            //{
                if (serieName == "all")
                {
                    findSerie = db.serie.ToList();
                }
                else
                { 
                findSerie = from series in db.serie
                            where series.Name.Contains(serieName)
                            select series;
                findSerie = findSerie.ToList();
                }
            //}
            if (!findSerie.Any())
            {
                ViewBag.Result = "Серии не найдены";
            }
            return PartialView(findSerie);
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
