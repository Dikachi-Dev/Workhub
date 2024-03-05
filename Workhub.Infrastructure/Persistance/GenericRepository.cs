
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
        _context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = _context.Set<TEntity>();
    }

    public async Task Add(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task<TEntity> GetById(string Id)
    {
        return await DbSet.FindAsync(Id);
    }

    public IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public async Task Delete(string Id)
    {
        TEntity entity = await GetById(Id);
        DbSet.Remove(entity);
    }

    public async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}


