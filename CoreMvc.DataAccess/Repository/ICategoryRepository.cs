
using CoreMvc.DataAccess.Data;
using CoreMvc.Models;

namespace CoreMvc.DataAccess.Repository;
public interface ICategoryRepository : IRepository<Category>
{
  void Update(Category category);
}
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
  private ApplicationDbContext _dbContext;
  public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }
  public void Update(Category category)
  {
    _dbContext.Categories.Update(category);
  }
}