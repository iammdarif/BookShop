using BookShop.Data;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name?.ToLower() == category.DisplayOrder.ToString())
            { 
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int? id) 
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            var category = _db.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category) 
        {
            if (ModelState.IsValid)
            {
                var categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == category.Id);

                if (categoryFromDb == null)
                {
                    return NotFound();
                }


                categoryFromDb.Name = category.Name;
                categoryFromDb.DisplayOrder = category.DisplayOrder;

                _db.Categories.Update(categoryFromDb);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";

                return RedirectToAction(nameof(Index));

            }

            return View(category);
        }

        public IActionResult Delete(int? id) 
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            var category = _db.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category) 
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == category.Id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(categoryFromDb);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
