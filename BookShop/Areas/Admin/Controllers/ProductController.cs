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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            
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

                string webRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) 
                { 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(webRootPath, @"images\products");


                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        //adding a new image, delete the old image
                        var oldImagePath = Path.Combine(webRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVm.Product.ImageUrl = @"\images\products\" + fileName;
                }

                if (productVm.Product.Id == 0) 
                {
                    //adding a product
                    _unitOfWork.Product.Add(productVm.Product);
                }
                else
                {
                    //updating a product
                    _unitOfWork.Product.Update(productVm.Product);
                }

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
        //public IActionResult Delete(int id)
        //{
        //    var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productFromDb != null)
        //    {
        //        return View(productFromDb);
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Delete(Product product)
        //{
        //    _unitOfWork.Product.Remove(product);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product deleted successfully";
        //    return RedirectToAction(nameof(Index));
            
        //}


        #region APICalls

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productTobeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

            if (productTobeDeleted == null)
            {
                return Json(new { success = false, message = "product not found, error deleting"});
            }


            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productTobeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productTobeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "delete successful" });
        }

        #endregion
    }
}
