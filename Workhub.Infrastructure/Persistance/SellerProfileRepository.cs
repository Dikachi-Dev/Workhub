using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Persistance;
    public class SellerProfileRepository : ISellerProfileRepository
    {
        public void Add(SellerProfile entity)
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

        public IQueryable<SellerProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public SellerProfile GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SellerProfile?> GetQueryableSellerProfiles()
        {
            throw new NotImplementedException();
        }

        public SellerProfile? GetSellerProfileByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public SellerProfile? GetSellerProfileById(string id)
        {
            throw new NotImplementedException();
        }

        public SellerProfile? GetSellerProfileByIdAllWithCollections(string id)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(SellerProfile entity)
        {
            throw new NotImplementedException();
        }
    }

