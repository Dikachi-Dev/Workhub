
using Microsoft.EntityFrameworkCore;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly AppDataContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(AppDataContext context)
    {
        _context = context;
        DbSet = _context.Set<TEntity>(); // Initialize DbSet
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void Delete(string Id)
    {
        DbSet.Remove(GetById(Id));
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public TEntity GetById(string Id)
    {
        return DbSet.Find(Id);
    }

    public int SaveChanges() => _context.SaveChanges();

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }
}
