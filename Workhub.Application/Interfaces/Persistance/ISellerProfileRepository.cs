using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;
public interface ISellerProfileRepository : IGenericRepository<SellerProfile>
{
    SellerProfile? GetSellerProfileByEmail(string email);
    SellerProfile? GetSellerProfileById(string id);
    IQueryable<SellerProfile?> GetSellerFilter(string filter);
    SellerProfile? GetSellerProfileByIdAllWithCollections(string id);
    IQueryable<SellerProfile?> GetQueryableSellerProfiles();
}

