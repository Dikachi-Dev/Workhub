namespace Workhub.Application.Interfaces.Persistance;
public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
{
    Task Add(TEntity entity);
    Task<TEntity> GetById(string Id);
    IQueryable<TEntity> GetAll();
    void Update(TEntity entity);
    Task Delete(string Id);
    Task<int> SaveChanges();
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

