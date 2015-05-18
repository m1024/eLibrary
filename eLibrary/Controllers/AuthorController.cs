using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class AuthorController : Controller
    {

        private eLibraryContext db = new eLibraryContext();

        /// <summary>
        /// Вывод списка всех авторов, постранично
        /// </summary>
        /// <param name="id">Номер страницы</param>
        /// <returns>Страница со списком всех авторов</returns>
        [AllowAnonymous]
        public ActionResult Index(int? id)
        {
            if (id < 0) id = 0;
            else if (id > db.author.Count() / 10) id = db.author.Count() / 10;
            var authors = new object();
            if (id == null)
                authors = db.author.OrderBy(i => i.Id).Take(10).ToList();
            else
            {
                authors = db.author.OrderBy(i => i.Id).Skip((int)id * 10).Take(10).ToList();
            }
            ViewBag.page = id == null ? 0 : id;
            ViewBag.lastPage = db.author.Count() / 10;
            return View(authors);

        }

        /// <summary>
        /// Редактирование автора
        /// </summary>
        /// <param name="id">Id записи в бд</param>
        /// <returns>Страница редактирования автора</returns>
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Edit(int id = 0)
        {
            Author author = db.author.Find(id);

            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        /// <summary>
        /// Редактирование автора
        /// </summary>
        /// <param name="author">Измененные пользователем данные об авторе</param>
        /// <param name="uploadImage">Прикрепленное изображение автора</param>
        /// <returns>Перенаправление на страницу всех авторов</returns>
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public ActionResult Edit(Author author, HttpPostedFileBase uploadImage)
        {

            if (uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                author.Image = imageData;
            }

            db.Entry(author).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создание автора
        /// </summary>
        /// <returns>Страница создания автора</returns>
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Создание автора
        /// </summary>
        /// <param name="author">Введенные пользователем данные об авторе</param>
        /// <param name="uploadImage">Изображение автора</param>
        /// <returns>Страница добавленного автора</returns>
        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create(Author author, HttpPostedFileBase uploadImage)
        {

            if (uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                author.Image = imageData;
            }

            if (ModelState.IsValid)
            {
                db.author.Add(author);
                db.SaveChanges();
                return RedirectToAction("ShowAuthor", "Author", 
                    new { id = db.author.Where(u => u.Family == author.Family).FirstOrDefault().Id });
            }

            return View(author);
        }

        /// <summary>
        /// Удаление автора
        /// </summary>
        /// <param name="id">Id записи автора в бд</param>
        /// <returns>Частичное представление для подтверждения</returns>
        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Author b = db.author.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return PartialView(b);
        }

        /// <summary>
        /// Подтверждение удаления автора
        /// </summary>
        /// <param name="id">Id записи автора в бд</param>
        /// <returns>Страница всех авторов</returns>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Author b = db.author.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.author.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Отображение подробной информации об авторе
        /// </summary>
        /// <param name="id">Id автора в бд</param>
        /// <returns>Страница автора</returns>
        [AllowAnonymous]
        public ActionResult ShowAuthor(int? id)
        {
            if (id != null)
            {
                Author author = db.author.Find(id);
                if (author == null) return HttpNotFound();

                return View(author);
            }
            else
                return View();

        }

        /// <summary>
        /// Выборка книг заданного автора
        /// </summary>
        /// <param name="id">Id автора в бд</param>
        /// <returns>Частичное представление - список книг автора</returns>
        [AllowAnonymous]
        public ActionResult BooksByAuthor(int id)
        {
            IEnumerable<Book> books = db.author.Find(id).Books;
            int i = 0;
            Book[] rezBooks = new Book[books.Count()];
            foreach (var b in books)
            {
                rezBooks[i] = db.book.Include(u=>u.Genre).Include(u=>u.Serie).FirstOrDefault(u => u.Id == b.Id);
                i++;
            }
            ViewBag.rez = rezBooks;

            return PartialView();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
