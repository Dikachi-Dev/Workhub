using System.Data.Entity;
using System.Data.Entity.Migrations;
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
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void Delete(string Id)
    {
        DbSet.Remove(DbSet.Find(Id));
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

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        DbSet.AddOrUpdate(entity);
    }
}