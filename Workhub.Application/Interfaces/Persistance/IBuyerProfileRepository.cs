using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IBuyerProfileRepository : IGenericRepository<BuyerProfile>
{
    BuyerProfile? GetBuyerProfileByEmail(string email);

    BuyerProfile? GetBuyerProfileById(string id);

    IQueryable<BuyerProfile?> GetBuyerFilter(string filter);

    BuyerProfile? GetBuyerProfileByIdAllWithCollections(string id);

    IQueryable<BuyerProfile?> GetQueryableBuyerProfiles();
}