using System.Data;
using System.Linq.Expressions;
using CoreMvc.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreMvc.DataAccess.Repository;
public interface IRepository<T> where T : class
{
  IEnumerable<T> GetAll(string? includeProperties = null);
  T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
  void Add(T entity);
  void Remove(T entity);
  void RemoveRange(IEnumerable<T> entity);
}

public class Repository<T> : IRepository<T> where T : class
{
  private readonly ApplicationDbContext _dbContext;
  internal DbSet<T> dbSet;
  public Repository(ApplicationDbContext dbContext)
  {
    _dbContext = dbContext;
    this.dbSet = _dbContext.Set<T>();
    _dbContext.Products.Include(u => u.Category).Include(u => u.CategoryId);
  }
  public void Add(T entity)
  {
    dbSet.Add(entity);
  }

  public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
  {
    IQueryable<T> query = dbSet;
    if (!string.IsNullOrEmpty(includeProperties))
    {
      foreach (var includePropertie in includeProperties
            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(includePropertie);
      }
    }
    query = query.Where(filter);
    return query.FirstOrDefault();
  }

  public IEnumerable<T> GetAll(string? includeProperties = null)
  {
    IQueryable<T> query = dbSet;
    if (!string.IsNullOrEmpty(includeProperties))
    {
      foreach (var includePropertie in includeProperties
            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(includePropertie);
      }
    }
    return query.ToList();
  }

  public void Remove(T entity)
  {
    dbSet.Remove(entity);
  }

  public void RemoveRange(IEnumerable<T> entity)
  {
    dbSet.RemoveRange(entity);
  }
}