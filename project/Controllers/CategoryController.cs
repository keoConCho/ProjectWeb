﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using project.Data;
using project.Models;

namespace project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Remove(int id)
        {
            var category = context.Categories.Find(id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = context.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Update(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = context.Categories
                                    .Include(c => c.Books)
                                    .FirstOrDefault(c => c.Id == id);
            //Note: khi muốn truy xuất dữ liệu của bảng B từ bảng A
            //thì cần sử dụng Include kết hợp với FirstOrDefault
            //còn nếu chỉ truy xuất thông tin id đơn thuần thì sử dụng
            //Find hoặc FirstOrDefault đều được
            return View(category);
        }
    }
}
