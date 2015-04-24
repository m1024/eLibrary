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
    public class BookController : Controller
    {
        private eLibraryContext db = new eLibraryContext();
        //
        // GET: /BookAuthor/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var books = db.book.Include(u => u.Genre).Include(u => u.Serie).ToList();
            return View(books);
        }

        [AllowAnonymous]
        public ActionResult ShowBook(int? id)
        {
            if (id != null)
            {
                Book book = db.book.Find(id);
                if (book == null) return HttpNotFound();
                //также надо передать наименование жанра и серии, а не ID
                book.Genre = db.genre.Find(book.GenreId);
                book.Serie = db.serie.Find(book.SerieId);
                return View(book);
            }
            else
                return View();

        }

        [Authorize]
        public FileContentResult GetFile_fb2(int id = 0)
        {
            Book book = db.book.Find(id);
            return new FileContentResult(book.Text_fb2,"text");
        }

        [Authorize]
        public FileContentResult GetFile_txt(int id = 0)
        {
            Book book = db.book.Find(id);
            return new FileContentResult(book.Text_txt, "text");
        }

        [Authorize]
        public FileContentResult GetFile_epub(int id = 0)
        {
            Book book = db.book.Find(id);
            return new FileContentResult(book.Text_epub, "text");
        }

                [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Edit(int id = 0)
        {
            Book book = db.book.Find(id);

            SelectList genres = new SelectList(db.genre, "Id", "Name");
            ViewBag.Genres = genres;

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Edit(Book book, int[] selectAuthor, HttpPostedFileBase uploadImage,
            HttpPostedFileBase uploadText_fb2, HttpPostedFileBase uploadText_txt, HttpPostedFileBase uploadText_epub, int? selectSerie)
        {
                book.Authors.Clear();

                Book newBook = db.book.Find(book.Id);

                newBook.Name = book.Name;
                newBook.GenreId = book.GenreId;
                newBook.SerieId = book.SerieId;
                newBook.ImprintYear = book.ImprintYear;
                newBook.Annotation = book.Annotation;

                if (selectAuthor != null)
                {
                    newBook.Authors.Clear();
                    foreach (var c in db.author.Where(co => selectAuthor.Contains(co.Id)))
                    {
                        book.Authors.Add(c);
                    }
                    newBook.Authors = book.Authors;
                }

                if (uploadImage != null)
                {
                    byte[] uploadImageNew = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        uploadImageNew = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // установка массива байтов
                    newBook.Image = uploadImageNew;
                }

                if (uploadText_fb2 != null)
                {
                    byte[] uploadTextFb2 = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadText_fb2.InputStream))
                    {
                        uploadTextFb2 = binaryReader.ReadBytes(uploadText_fb2.ContentLength);
                    }
                    // установка массива байтов
                    newBook.Text_fb2 = uploadTextFb2;
                }


                if (uploadText_txt != null)
                {
                    byte[] uploadTextTxt = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadText_txt.InputStream))
                    {
                        uploadTextTxt = binaryReader.ReadBytes(uploadText_txt.ContentLength);
                    }
                    // установка массива байтов
                    newBook.Text_txt = uploadTextTxt;
                }


                if (uploadText_epub != null)
                {
                    byte[] uploadTextEpub = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadText_epub.InputStream))
                    {
                        uploadTextEpub = binaryReader.ReadBytes(uploadText_epub.ContentLength);
                    }
                    // установка массива байтов
                    newBook.Text_epub = uploadTextEpub;
                }

                if (selectSerie != null)
                {
                    newBook.SerieId = (int) selectSerie;
                }
                else newBook.SerieId = 1;


                db.Entry(newBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Администратор, Модератор")]
        //public ActionResult Edit(int id = 0)
        //{
        //    Book book = db.book.Find(id);

        //    SelectList genres = new SelectList(db.genre, "Id", "Name");
        //    ViewBag.Genres = genres;

        //    SelectList series = new SelectList(db.serie, "Id", "Name");
        //    ViewBag.Series = series;

        //    if (book == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(book);
        //}

        //[Authorize(Roles = "Администратор, Модератор")]
        //[HttpPost]
        //public ActionResult Edit(Book book, string[] selectedAuthors, HttpPostedFileBase uploadImage, HttpPostedFileBase uploadText_fb2)
        //{
        //    Book newBook = db.book.Find(book.Id);

        //    newBook.Name = book.Name;
        //    newBook.GenreId = book.GenreId;
        //    newBook.SerieId = book.SerieId;
        //    newBook.ImprintYear = book.ImprintYear;
        //    newBook.Annotation = book.Annotation;

        //    if (uploadImage != null)
        //    {
        //        byte[] imageData = null;
        //        // считываем переданный файл в массив байтов
        //        using (var binaryReader = new BinaryReader(uploadImage.InputStream))
        //        {
        //            imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
        //        }
        //        // установка массива байтов
        //        newBook.Image = imageData;
        //    }
        //    if (uploadText_fb2 != null)
        //    {
        //        byte[] uploadTextFb2 = null;
        //        // считываем переданный файл в массив байтов
        //        using (var binaryReader = new BinaryReader(uploadText_fb2.InputStream))
        //        {
        //            uploadTextFb2 = binaryReader.ReadBytes(uploadText_fb2.ContentLength);
        //        }
        //        // установка массива байтов
        //        newBook.Text_fb2 = uploadTextFb2;
        //    }

        //    newBook.Authors.Clear();
        //    if (selectedAuthors != null)
        //    {
        //        //получаем выбранных авторов (потом может быть будем парсить из строки)
        //        //foreach (var c in db.author.Where(co => selectedAuthors.Contains(co.Id)))
        //        //{
        //        //    newBook.Authors.Add(c);
        //        //}
        //        foreach (var c in db.author.Where(co => selectedAuthors.Contains(co.Family)))
        //        {
        //            newBook.Authors.Add(c);
        //        }
        //    }

        //    db.Entry(newBook).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create()
        {
            SelectList genres = new SelectList(db.genre, "Id", "Name");
            ViewBag.Genres = genres;

            SelectList series = new SelectList(db.serie, "Id", "Name");
            ViewBag.Series = series;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create(Book book,  int[] selectAuthor, HttpPostedFileBase uploadImage, HttpPostedFileBase uploadText_fb2,
            HttpPostedFileBase uploadText_txt, HttpPostedFileBase uploadText_epub, int? selectSerie)
        {
            //if (ModelState.IsValid) //почему-то не проходит когда все хорошо
            if (book.Name!=null && book.GenreId >= 1)
            {
                book.Authors.Clear();

                if (selectAuthor != null)
                {
                    foreach (var c in db.author.Where(co => selectAuthor.Contains(co.Id)))
                    {
                        book.Authors.Add(c);
                    }
                }

                if (uploadImage != null)
                {
                    byte[] uploadImageNew = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        uploadImageNew = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    // установка массива байтов
                    book.Image = uploadImageNew;
                }

                if (uploadText_fb2 != null)
                {
                    byte[] uploadTextFb2 = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadText_fb2.InputStream))
                    {
                        uploadTextFb2 = binaryReader.ReadBytes(uploadText_fb2.ContentLength);
                    }
                    // установка массива байтов
                    book.Text_fb2 = uploadTextFb2;
                }


                if (uploadText_txt != null)
                {
                    byte[] uploadTextTxt = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadText_txt.InputStream))
                    {
                        uploadTextTxt = binaryReader.ReadBytes(uploadText_txt.ContentLength);
                    }
                    // установка массива байтов
                    book.Text_txt = uploadTextTxt;
                }


                if (uploadText_epub != null)
                {
                    byte[] uploadTextEpub = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadText_epub.InputStream))
                    {
                        uploadTextEpub = binaryReader.ReadBytes(uploadText_epub.ContentLength);
                    }
                    // установка массива байтов
                    book.Text_epub = uploadTextEpub;
                }

                if (selectSerie != null)
                {
                    book.SerieId = (int) selectSerie;
                }
                else book.SerieId = 1;


                db.book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            SelectList genres = new SelectList(db.genre, "Id", "Name");
            ViewBag.Genres = genres;

            SelectList series = new SelectList(db.serie, "Id", "Name");
            ViewBag.Series = series;


            return View(book);
        }


        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Book b = db.book.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Book b = db.book.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.book.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AuthorSearch(string authorName)
        {
            IEnumerable<Author> findAuthor = null;
            if (authorName != null)
            {
                findAuthor = from author in db.author
                             where author.Family == authorName
                             select author;
            }
            if (findAuthor == null)
            {
                return HttpNotFound();
            }
            return PartialView(findAuthor.ToList());
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

        public ActionResult Read(int id, int? pageNumber) //pageNumber с нуля
        {
            Book book = db.book.Find(id);

            if (pageNumber == null) pageNumber = 0;

            byte[] page = new byte[10000]; //считываем по 10 Кбайт
            int startIndex = (int)pageNumber*10000;
            Array.Copy(book.Text_txt, startIndex, page, 0, 10000);

            ViewBag.page = new byte[10000];
            ViewBag.page = page;
            ViewBag.currentPage = pageNumber;

            return View(book);
        }
    }
}
