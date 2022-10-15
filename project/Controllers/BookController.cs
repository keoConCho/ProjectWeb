﻿

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using project.Data;
using project.Models;

namespace project.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext context;

        public BookController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View(context.Books.ToList());
        }

        //public IActionResult List()
        //{
        //    return View(context.Books.ToList());
        //}

        //[Authorize(Roles = "Administrator")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

                var book = context.Books.Find(id);

                context.Books.Remove(book);

                context.SaveChanges();


                TempData["Message"] = "Delete book successfully !";


                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Detail(int id)
        {
            var book = context.Books
                                 .Include(b => b.Category)
                                 .FirstOrDefault(b => b.Id == id);
            return View(book);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                context.Books.Add(book);
                context.SaveChanges();
                TempData["Message"] = "Add book successfully !";
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                return View(book);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            return View(context.Books.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                context.Books.Update(book);
                context.SaveChanges();
                TempData["Message"] = "Edit book successfully !";
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                return View(book);
            }
        }
    }
}
