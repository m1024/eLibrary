using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLibrary.Models;

namespace eLibrary.Controllers
{
    public class CommentsController : Controller
    {
        //комментарии к конкретной книге будем искать по id книги и передавать в частичное представление
        //которое будет отображаться на странице с книгой
        private eLibraryContext db = new eLibraryContext();

        [Authorize]
        public ActionResult ShowComments(int? bookId)
        {
            IEnumerable<Comment> findComments = null;
            if (bookId != null)
            {
                findComments = from comments in db.comments
                               where comments.BookId == (int)bookId
                               select comments;
            }

            //не работает!
            //foreach (var c in findComments)
            //{
            //    c.User = db.user.Find(c.UserId);
            //}

            if (findComments == null)
            {
                return HttpNotFound();
            }
            return PartialView(findComments.ToList());
        }


        [HttpPost]
        [Authorize]
        public ActionResult addComment(Comment comment)
        {
            foreach (var c in db.user)
            {
                if (HttpContext.User.Identity.Name == c.Name)
                    comment.UserId = c.Id;
            }
            comment.Time = System.DateTime.Now;
            db.comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("ShowComments");
        }

 
    
    }
}
