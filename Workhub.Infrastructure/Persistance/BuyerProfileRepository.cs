using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Persistance
{
    public class BuyerProfileRepository : IBuyerProfileRepository
    {
        public void Add(BuyerProfile entity)
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

        public IQueryable<BuyerProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public BuyerProfile? GetBuyerProfileByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public BuyerProfile? GetBuyerProfileById(string id)
        {
            throw new NotImplementedException();
        }

        public BuyerProfile? GetBuyerProfileByIdAllWithCollections(string id)
        {
            throw new NotImplementedException();
        }

        public BuyerProfile GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BuyerProfile?> GetQueryableBuyerProfiles()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(BuyerProfile entity)
        {
            throw new NotImplementedException();
        }
    }
}
