using System.Data.Common;
using CoreMvc.DataAccess.Repository;
using CoreMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreMvc.Area.Admin.Controllers
{
  [Area("Admin")]
  public class CategoryController : Controller
  {
    private readonly IUnitOfWorks _unitOfWork;
    public CategoryController(IUnitOfWorks unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
      List<Category> data = _unitOfWork.CategoryRepository.GetAll().ToList();
      return View(data);
    }
    public IActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public IActionResult Create(Category category)
    {
      if (ModelState.IsValid)
      {
        _unitOfWork.CategoryRepository.Add(category);
        _unitOfWork.Save();
        TempData["success"] = "Category created succesfully.";
        return RedirectToAction("Index");
      }
      return View();
    }
    public IActionResult Edit(int? id)
    {
      if (id is null || id == 0)
      {
        return NotFound();
      }
      Category category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
      if (category is null)
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
        _unitOfWork.CategoryRepository.Update(category);
        _unitOfWork.Save();
        TempData["success"] = "Category edited succesfully.";
        return RedirectToAction("Index");
      }
      return View();
    }
    public IActionResult Delete(int? id)
    {
      if (id is null || id == 0)
      {
        return NotFound();
      }
      Category category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
      if (category is null)
      {
        return NotFound();
      }
      return View(category);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
      Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
      if (category is null)
      {
        return NotFound();
      }
      _unitOfWork.CategoryRepository.Remove(category);
      _unitOfWork.Save();
      TempData["success"] = "Category deleted succesfully.";
      return RedirectToAction("Index");
    }
  }
}
