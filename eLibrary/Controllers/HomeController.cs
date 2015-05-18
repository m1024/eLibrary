using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class HomeController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        /// <summary>
        /// Вывод главной страницы приложения
        /// </summary>
        /// <returns>Главная страница</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="general">Слово для поиска</param>
        /// <returns>Страница расширенного поиска</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult FindBook(string general)
        {
            ViewBag.general = general;
            return View();
        }

        /// <summary>
        /// Поиск по автору
        /// </summary>
        /// <param name="authorName">Имя, Фамилия или Отчество автора</param>
        /// <returns>Частичное представление с результатом поиска</returns>
        public ActionResult AuthorSearch(string authorName)
        {
            List<Author> authorsList = new List<Author>();
            if (authorName != null)
            {
                string[] words = authorName.Split(new char[] {' ', ','});

                IEnumerable<Author> newAuthors = null;
                foreach (var w in words)
                {
                    if (w.Trim() != "") 
                    newAuthors = from author in db.author
                                  where author.Family.Contains(w.Trim()) 
                                  || author.Name.Contains(w.Trim()) || author.Patronymic.Contains(w.Trim())
                                  select author;
                    if (newAuthors != null)
                    foreach (var author in newAuthors)
                    {
                        authorsList.Add(author);
                    }
                }
            }
            if (!authorsList.Any())
            {
                ViewBag.Result = "Авторы не найдены";
            }
            return PartialView(authorsList);
        }

        /// <summary>
        /// Поиск по книге
        /// </summary>
        /// <param name="bookName">Наименование книги</param>
        /// <returns>Частичное представление с результатом поиска</returns>
        public ActionResult BookSearch(string bookName)
        {
            IEnumerable<Book> findBook = null;
            if (bookName != null)
            {
                findBook = from book in db.book.Include(u => u.Serie).Include(u => u.Genre)
                           where book.Name.Contains(bookName)
                           select book;
                findBook = findBook.ToList();
            }
            if (!findBook.Any())
            {
                ViewBag.Result = "Книги не найдены";
            }
            return PartialView(findBook);
        }

        /// <summary>
        /// Выборка книг по жанру
        /// </summary>
        /// <param name="genreId">Id жанра в бд</param>
        /// <returns>Частичное представление с результатом поиска</returns>
        public ActionResult SelectGenre(int? genreId)
        {
            IEnumerable<Book> findBook = null;
            if (genreId != null)
            {
                findBook = from book in db.book.Include(u => u.Serie).Include(u => u.Genre)
                           where book.GenreId == (int)genreId
                           select book;
            }
            if (findBook == null)
            {
                return HttpNotFound();
            }
            return View(findBook.ToList());
        }

        /// <summary>
        /// Выборка всех жанров
        /// </summary>
        /// <returns>Частичное представление со всеми жанрами</returns>
        [AllowAnonymous]
        public ActionResult Genres()
        {
            var genres = db.genre.ToList();
            return PartialView(genres);
        }


        /// <summary>
        /// Выборка 10 наиболее скачиваемых книг
        /// </summary>
        /// <returns>Частичное представление с результатом выборки</returns>
        public ActionResult MostDownloaded()
        {
            var books = db.book.OrderByDescending(i => i.Downloads).ToList().Take(10);
            return PartialView(books);
        }

        /// <summary>
        /// Перенаправление с поиска на главной странице
        /// </summary>
        /// <param name="searchText">Тект для поска</param>
        /// <returns>Перенаправление на поиск</returns>
        public RedirectToRouteResult RedirectToFind(string searchText)
        {
            return RedirectToAction("FindBook", "Home", new { general = searchText });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
