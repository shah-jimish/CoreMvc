using CoreMvcRazor.Data;
using CoreMvcRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreMvcRazor.Pages.Categories
{
  public class DeleteModel : PageModel
  {
    private readonly ApplicationDbContext _dbContext;
    [BindProperty]
    public Category Category { get; set; }
    public DeleteModel(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public void OnGet(int? id)
    {
      Category = _dbContext.Categories.Find(id);
    }
    public IActionResult OnPost()
    {
      Category? category = _dbContext.Categories.Find(Category.Id);
      if (category is null)
      {
        return NotFound();
      }
      _dbContext.Categories.Remove(category);
      _dbContext.SaveChanges();
      TempData["success"] = "Category deleted succesfully.";
      return RedirectToPage("Index");
    }
  }
}
