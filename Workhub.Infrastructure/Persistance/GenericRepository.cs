
using Microsoft.EntityFrameworkCore;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly AppDataContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(AppDataContext context, CancellationToken token)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = _context.Set<TEntity>();
    }

    public async Task Add(TEntity entity, CancellationToken token)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task<TEntity> GetById(string Id, CancellationToken token)
    {
        return await DbSet.FindAsync(Id, token);
    }

    public IQueryable<TEntity> GetAll(CancellationToken token)
    {
        return DbSet;
    }

    public void Update(TEntity entity, CancellationToken token)
    {
        DbSet.Update(entity);
    }

    public async Task Delete(string Id, CancellationToken token)
    {
        TEntity entity = await GetById(Id, token);
        DbSet.Remove(entity);
    }

    public async Task<int> SaveChanges(CancellationToken token)
    {
        return await _context.SaveChangesAsync(token);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}


