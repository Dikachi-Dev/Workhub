using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Infrastructure.Persistance;
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity)
        {
            
            throw new NotImplementedException();
        }

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }

