using CoreMvc.DataAccess.Data;

namespace CoreMvc.DataAccess.Repository
{
  public interface IUnitOfWorks
  {
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    void Save();
  }

  public class UnitOfWorks : IUnitOfWorks
  {
    private ApplicationDbContext _dbContext;
    public ICategoryRepository CategoryRepository { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    public UnitOfWorks(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
      CategoryRepository = new CategoryRepository(_dbContext);
      ProductRepository = new ProductRepository(_dbContext);
    }

    public void Save()
    {
      _dbContext.SaveChanges();
    }
  }
}