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

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult FindBook()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult FindBook(string bookName)
        {
            IEnumerable<Book> findBook = null;
            if (bookName != null)
            {
                findBook = from book in db.book.Include(u => u.Serie).Include(u=>u.Genre)
                   where book.Name==bookName
                   select book;
            }

            return View(findBook.ToList());
        }

        public ActionResult BookSearch(string bookName)
        {
            IEnumerable<Book> findBook = null;
            if (bookName != null)
            {
                findBook = from book in db.book.Include(u => u.Serie).Include(u => u.Genre)
                           where book.Name == bookName
                           select book;
            }
            if (findBook == null)
            {
                return HttpNotFound();
            }
            return PartialView(findBook.ToList());
        }


    }
}
