namespace Workhub.Application.Interfaces.Persistance;
public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
{
    Task Add(TEntity entity, CancellationToken token);
    Task<TEntity> GetById(string Id, CancellationToken token);
    IQueryable<TEntity> GetAll(CancellationToken token);
    void Update(TEntity entity, CancellationToken token);
    Task Delete(string Id, CancellationToken token);
    Task<int> SaveChanges(CancellationToken token);
}

//public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
//{
//    void Add(TEntity entity);
//    TEntity GetById(string Id);
//    IQueryable<TEntity> GetAll();
//    void Update(TEntity entity);
//    void Delete(string Id);
//    int SaveChanges();
//}

