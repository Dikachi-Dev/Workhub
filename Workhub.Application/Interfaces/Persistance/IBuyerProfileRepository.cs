using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IBuyerProfileRepository : IGenericRepository<BuyerProfile>
{
    BuyerProfile? GetBuyerProfileByEmail(string email);

    BuyerProfile? GetBuyerProfileById(string id);

    BuyerProfile? GetBuyerProfileByIdAllWithCollections(string id);

    IQueryable<BuyerProfile?> GetQueryableBuyerProfiles();
}