using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVm = new() { 
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                        new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        })
            };

            if (id == null || id == 0)
            {
                //create product
                return View(productVm);
            }
            else
            {
                //update product
                productVm.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVm);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product added successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                        new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        });
                return View(productVm);
            }
        }
        public IActionResult Delete(int id)
        {
            var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb != null)
            {
                return View(productFromDb);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction(nameof(Index));
            
        }
    }
}
