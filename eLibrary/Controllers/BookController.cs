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

        /// <summary>
        /// Отображает список всех книг из бд
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns>html страница</returns>
        [AllowAnonymous]
        public ActionResult Index(int? id)
        {
            if (id < 0) id = 0;
            else if (id > db.book.Count() / 10) id = db.book.Count() / 10;
            var books = new object();
            if (id == null)
                books = db.book.Include(u => u.Genre).Include(u => u.Serie).OrderBy(i => i.Id).Take(10).ToList();
            else
            {
                books = db.book.Include(u => u.Genre).Include(u => u.Serie).OrderBy(i => i.Id).Skip((int)id * 10).Take(10).ToList();
            }
            ViewBag.page = id == null ? 0 : id;
            ViewBag.lastPage = db.book.Count()/10;
            return View(books);
        }

        /// <summary>
        /// Отображает страницу для книги
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns>html страница</returns>
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
            book.Downloads += 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return new FileContentResult(book.Text_fb2,"text");
        }

        [Authorize]
        public FileContentResult GetFile_txt(int id = 0)
        {
            Book book = db.book.Find(id);
            book.Downloads += 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return new FileContentResult(book.Text_txt, "text");
        }

        [Authorize]
        public FileContentResult GetFile_epub(int id = 0)
        {
            Book book = db.book.Find(id);
            book.Downloads += 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
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
        public RedirectToRouteResult Edit(Book book, int[] selectAuthor, HttpPostedFileBase uploadImage,
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
                return RedirectToAction("ShowBook", "Book", new { id = newBook.Id });
        }

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
        public RedirectToRouteResult Create(Book book,  int[] selectAuthor, HttpPostedFileBase uploadImage, HttpPostedFileBase uploadText_fb2,
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
                book.Downloads = 0;

                db.book.Add(book);
                db.SaveChanges();

                IEnumerable<Book> findBook = from bookT in db.book.Include(u => u.Serie).Include(u => u.Genre)
                           where bookT.Name == book.Name
                           select bookT;

                return RedirectToAction("ShowBook", "Book", new { id = db.book.Where(u => u.Name == book.Name).FirstOrDefault().Id});
            }


            SelectList genres = new SelectList(db.genre, "Id", "Name");
            ViewBag.Genres = genres;

            SelectList series = new SelectList(db.serie, "Id", "Name");
            ViewBag.Series = series;


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
            Book b = db.book.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return PartialView(b);
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
                             where author.Family.Contains(authorName)
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
                             where series.Name.Contains(serieName)
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

            if (pageNumber == null || pageNumber < 0) pageNumber = 0;

            byte[] page = new byte[10000]; //считываем по 10 Кбайт

            int startIndex = 0;
            if ((int) pageNumber*10000 > book.Text_txt.Length)
            {
                startIndex = book.Text_txt.Length - 10000;
            }
            else
            {
                startIndex = (int)pageNumber * 10000;
                if (pageNumber > 0)
                    //теперь скорректируем чтобы с абзаца начать
                    for (int i = 0; i < 3000; i++)
                    {
                        if (book.Text_txt[startIndex - i] == 10 || book.Text_txt[startIndex - i] == 13)
                        {
                            startIndex -= i;
                            break;
                        }
                    }
            }
            
            if (startIndex + 10000 < book.Text_txt.Length)
                Array.Copy(book.Text_txt, startIndex, page, 0, 10000);
            else
            {
                Array.Copy(book.Text_txt, startIndex, page, 0, book.Text_txt.Length - startIndex);
            }

            //оборачиваем в теги <p> вместо символов /r
            string newPage = System.Text.Encoding.UTF8.GetString(page);
            string[] split = newPage.Split(new Char[] { '\r' }); //делим на абзацы 
            Array.Clear(split,split.Length-1,1); //выкидываем последний неполный абзац

            ViewBag.split = split.ToList();
            ViewBag.currentPage = pageNumber;
            ViewBag.lastPage = book.Text_txt.Length/10000;

            return View(book);
        }
    }
}
