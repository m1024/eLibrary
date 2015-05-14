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

        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create()
        {
            return View();
        }

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
                return RedirectToAction("Index");
            }

            return View(author);
        }


        [HttpGet]
        [Authorize(Roles = "Администратор")]
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

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Администратор")]
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



        // Просмотр подробных сведений о книге
        public ActionResult Details(int id)
        {
            Author author = db.author.Find(id);
            if (author != null)
            {
                return PartialView("_Delete", author);
            }
            return View("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
