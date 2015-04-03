﻿using System;
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
    public class AuthorController : Controller
    {

        private eLibraryContext db = new eLibraryContext();

        //
        // GET: /Author/

        [AllowAnonymous]
        public ActionResult Index()
        {
            var authors = db.author.ToList();
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
            return View(b);
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
    }

}