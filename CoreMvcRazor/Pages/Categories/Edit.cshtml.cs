using CoreMvcRazor.Data;
using CoreMvcRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreMvcRazor.Pages.Categories
{
  public class EditModel : PageModel
  {
    private readonly ApplicationDbContext _dbContext;
    [BindProperty]
    public Category Category { get; set; }
    public EditModel(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public void OnGet(int? id)
    {
      if (id > 0 || id != null)
      {
        Category = _dbContext.Categories.Find(id);
      }
    }
    public IActionResult OnPost()
    {
      if (ModelState.IsValid)
      {
        _dbContext.Categories.Update(Category);
        _dbContext.SaveChanges();
        TempData["success"] = "Category edited succesfully.";
        return RedirectToPage("Index");
      }
      return Page();
    }
  }
}
