using CoreMvcRazor.Data;
using CoreMvcRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreMvcRazor.Pages.Categories
{
  public class IndexModel : PageModel
  {
    private readonly ApplicationDbContext _dbContext;
    public List<Category> CateogoryList { get; set; }
    public IndexModel(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public void OnGet()
    {
      CateogoryList = _dbContext.Categories.ToList();
    }
  }
}
