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

        /// <summary>
        /// Выборка всех серий
        /// </summary>
        /// <returns>Страница со списком всех серий</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            var series = db.serie.ToList();
            return View(series);
        }

        /// <summary>
        /// Выборка всех серий
        /// </summary>
        /// <returns>Страница для работы с сериями модератору и администратору</returns>
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Main()
        {
            var series = db.serie.ToList();
            return View(series);
        }

        /// <summary>
        /// Создание серии
        /// </summary>
        /// <returns>Страница создания серии</returns>
        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// Создание серии
        /// </summary>
        /// <param name="serie">Введенны пользователем данные о добавляемой серии</param>
        /// <returns>Страница работы с сериями</returns>
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

        /// <summary>
        /// Редактирование серии
        /// </summary>
        /// <param name="id">Id серии в бд</param>
        /// <returns>Частичное представление для редактирования серии</returns>
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

        /// <summary>
        /// Редактирование серии
        /// </summary>
        /// <param name="serie">Измененные пользователем данные о серии</param>
        /// <returns>Страница новой серии</returns>
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

        /// <summary>
        /// Удаление серии
        /// </summary>
        /// <param name="id">Id серии в бд</param>
        /// <returns>Частичное представление для подтверждения удаления</returns>
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

        /// <summary>
        /// Подтверждение удаления
        /// </summary>
        /// <param name="id">Id серии в бд</param>
        /// <returns>Страница серий</returns>
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

        /// <summary>
        /// Поиск серии
        /// </summary>
        /// <param name="serieName">Наименование серии</param>
        /// <returns>Частичное представление с результатами поиска</returns>
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

        /// <summary>
        /// Отображение подробной информации о серии
        /// </summary>
        /// <param name="id">Id серии в бд</param>
        /// <returns>Страница серии</returns>
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

        /// <summary>
        /// Выбрка книг серии
        /// </summary>
        /// <param name="id">Id серии в бд</param>
        /// <returns>Частичное представление с результатами выборки</returns>
        [AllowAnonymous]
        public ActionResult BooksBySerie(int id)
        {
            IEnumerable<Book> findBooks = from books in db.book
                                          where books.SerieId == id
                                          select books;
            return PartialView(findBooks.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
