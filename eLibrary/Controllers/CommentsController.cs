using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class CommentsController : Controller
    {
        private eLibraryContext db = new eLibraryContext();

        /// <summary>
        /// Поиск и возвращение найденных комментариев
        /// </summary>
        /// <param name="bookId">Id книги в бд</param>
        /// <returns>Частичное представление с результатами поиска</returns>
        [AllowAnonymous]
        public ActionResult ShowComments(int? bookId)
        {
            IEnumerable<Comment> findComments = null;
            if (bookId != null)
            {
                findComments = from comments in db.comments
                               where comments.BookId == (int)bookId
                               select comments;
            }

            //находим пользователей для комментариев чтобы отобразить их имена во view
            if (findComments != null)
            for (int i = 0; i < findComments.Count(); i++)
                findComments.ElementAt(i).User = db.user.Find(findComments.ElementAt(i).UserId);


            if (findComments == null)
            {
                return HttpNotFound();
            }
            return PartialView(findComments.ToList());
        }

        /// <summary>
        /// Добавление комментария
        /// </summary>
        /// <param name="comment">Комментарий пользователя</param>
        /// <returns>Перенаправление на метод отображения комментариев</returns>
        [HttpPost]
        [Authorize]
        public ActionResult addComment(Comment comment)
        {
            if (comment.Text != null)
            {
                foreach (var c in db.user)
                {
                    if (HttpContext.User.Identity.Name == c.Name)
                        comment.UserId = c.Id;
                }
                comment.Time = System.DateTime.Now;
                db.comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("ShowComments");
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id">Id книги к которой относится комментарий</param>
        /// <returns>Страница книги к которой относился комментарий</returns>
        [HttpGet]
        [Authorize(Roles = "Администратор, Модератор, Пользователь")]
        public RedirectToRouteResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Book");
            }

            Comment b = db.comments.Find(id);
            int bookId = b.BookId;
            db.comments.Remove(b);
            db.SaveChanges();

            return RedirectToAction("ShowBook", "Book", new { id = bookId });
        }
    }
}
