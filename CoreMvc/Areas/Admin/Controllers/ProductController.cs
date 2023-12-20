using CoreMvc.DataAccess.Repository;
using CoreMvc.Models;
using CoreMvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMvc.Area.Admin.Controllers
{
  [Area("Admin")]
  public class ProductController : Controller
  {
    private readonly IUnitOfWorks _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWorks unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
      _unitOfWork = unitOfWork;
      _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
      List<Product> data = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
      return View(data);
    }
    public IActionResult Upsert(int? id)
    {
      ProductViewModel productViewModel = new()
      {
        CategoryList = _unitOfWork.CategoryRepository.
        GetAll().Select(u => new SelectListItem
        {
          Text = u.Name,
          Value = u.Id.ToString()
        }),
        Product = new Product()
      };
      if (id == null || id == 0)
      {

        return View(productViewModel);
      }
      productViewModel.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
      return View(productViewModel);
    }
    [HttpPost]
    public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
    {
      if (ModelState.IsValid)
      {
        string wwwRootPath = _webHostEnvironment.WebRootPath;
        if (file != null)
        {
          string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
          string productPath = Path.Combine(wwwRootPath, "images", "product");

          if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
          {
            var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
              System.IO.File.Delete(oldImagePath);
            }
          }
          using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
          {
            file.CopyTo(fileStream);
          }
          productViewModel.Product.ImageUrl = @"/images/product/" + fileName;
        }
        if (productViewModel.Product.Id == 0)
        {
          _unitOfWork.ProductRepository.Add(productViewModel.Product);
        }
        else
        {
          _unitOfWork.ProductRepository.Update(productViewModel.Product);
        }
        _unitOfWork.Save();
        TempData["success"] = "Product created succesfully.";
        return RedirectToAction("Index");
      }
      else
      {
        productViewModel.CategoryList = _unitOfWork.CategoryRepository.
        GetAll().Select(u => new SelectListItem
        {
          Text = u.Name,
          Value = u.Id.ToString()
        });
        return View(productViewModel);
      }
    }
    #region APICALLS
    [HttpGet]
    public IActionResult GetAll()
    {
      List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
      return Json(new { data = objProductList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
      var productToBeDeleted = _unitOfWork.ProductRepository.Get(u => u.Id == id);
      if (productToBeDeleted == null)
      {
        return Json(new { Success = false, message = "Error while deleting." });
      }
      var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('/'));
      if (System.IO.File.Exists(oldImagePath))
      {
        System.IO.File.Delete(oldImagePath);
      }
      _unitOfWork.ProductRepository.Remove(productToBeDeleted);
      _unitOfWork.Save();
      return Json(new { success = true, message = "Delete Succesfully." });
    }
    #endregion
  }
}
