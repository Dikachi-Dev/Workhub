using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class BuyerProfileRepository : GenericRepository<BuyerProfile>, IBuyerProfileRepository
{
    public BuyerProfileRepository(AppDataContext context) : base(context)
    {
    }

    public BuyerProfile? GetBuyerProfileByEmail(string email)
    {
        return GetAll().FirstOrDefault(x => x.Email == email);
    }

    public BuyerProfile? GetBuyerProfileById(string id)
    {
        return GetById(id);
    }

    public BuyerProfile? GetBuyerProfileByIdAllWithCollections(string id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<BuyerProfile?> GetQueryableBuyerProfiles()
    {
        throw new NotImplementedException();
    }
}