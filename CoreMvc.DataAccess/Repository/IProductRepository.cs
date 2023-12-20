
using CoreMvc.DataAccess.Data;
using CoreMvc.Models;

namespace CoreMvc.DataAccess.Repository;
public interface IProductRepository : IRepository<Product>
{
  void Update(Product product);
}
public class ProductRepository : Repository<Product>, IProductRepository
{
  private ApplicationDbContext _dbContext;
  public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }
  public void Update(Product product)
  {
    var productFromDb = _dbContext.Products.FirstOrDefault(u => u.Id == product.Id);
    if (productFromDb != null)
    {
      productFromDb.Title = product.Title;
      productFromDb.ISBN = product.ISBN;
      productFromDb.Price = product.Price;
      productFromDb.Price50 = product.Price50;
      productFromDb.ListPrice = product.ListPrice;
      productFromDb.Price100 = product.Price100;
      productFromDb.Description = product.Description;
      productFromDb.CategoryId = product.CategoryId;
      productFromDb.Author = product.Author;
      if (product.ImageUrl != null)
      {
        productFromDb.ImageUrl = product.ImageUrl;
      }
    }
  }
}