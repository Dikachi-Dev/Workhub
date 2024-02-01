using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class BuyerProfileRepository : GenericRepository<BuyerProfile>, IBuyerProfileRepository
{
    public BuyerProfileRepository(AppDataContext context) : base(context)
    {

    }

    public IQueryable<BuyerProfile?> GetBuyerFilter(string filter)
    {
        return GetAll()
            .Where(profile => profile != null && profile.FirstName
            .Contains(filter) || profile.Email
            .Contains(filter) || profile.LastName
            .Contains(filter) || profile.State
            .Contains(filter) || profile.Country
            .Contains(filter));
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