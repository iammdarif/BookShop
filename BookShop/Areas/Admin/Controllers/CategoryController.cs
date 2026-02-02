using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly ICategoryRepository _categoryRepository;

        //public CategoryController(ICategoryRepository db)
        public CategoryController(IUnitOfWork unitOfWork)
        {
            //_categoryRepository = db;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
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

            var category = _unitOfWork.Category.Get(u => u.Id == id);

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
                var categoryFromDb = _unitOfWork.Category.Get(u => u.Id == category.Id);

                if (categoryFromDb == null)
                {
                    return NotFound();
                }


                categoryFromDb.Name = category.Name;
                categoryFromDb.DisplayOrder = category.DisplayOrder;

                _unitOfWork.Category.Update(categoryFromDb);
                _unitOfWork.Save();
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

            var category = _unitOfWork.Category.Get(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category category) 
        {
            var categoryFromDb = _unitOfWork.Category.Get(u => u.Id == category.Id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
