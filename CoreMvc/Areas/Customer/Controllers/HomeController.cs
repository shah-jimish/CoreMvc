using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CoreMvc.Models;
using CoreMvc.DataAccess.Repository;

namespace CoreMvc.Area.Customer.Controllers;
[Area("Customer")]
public class HomeController : Controller
{
  private readonly IUnitOfWorks _unitOfWork;

  public HomeController(IUnitOfWorks unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public IActionResult Index()
  {
    IEnumerable<Product> productsList = _unitOfWork.ProductRepository.GetAll();
    return View(productsList);
  }
  public IActionResult Details(int productId)
  {
    Product product = _unitOfWork.ProductRepository.Get(u => u.Id == productId, includeProperties: "Category");
    return View(product);
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
