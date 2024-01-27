using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class SellerProfileRepository : GenericRepository<SellerProfile>, ISellerProfileRepository
{
    public SellerProfileRepository(AppDataContext context) : base(context)
    {
    }

    public IQueryable<SellerProfile?> GetSellerFilter(string filter)
    {
        return GetAll()
           .Where(profile => profile != null && profile.FirstName
           .Contains(filter) || profile.Email
           .Contains(filter) || profile.LastName
           .Contains(filter) || profile.State
           .Contains(filter) || profile.Country
           .Contains(filter));
    }

    public IQueryable<SellerProfile?> GetQueryableSellerProfiles()
    {
        throw new NotImplementedException();
    }

    public SellerProfile? GetSellerProfileByEmail(string email)
    {
        return GetAll().FirstOrDefault(x => x.Email == email);
    }

    public SellerProfile? GetSellerProfileById(string id)
    {
        return GetById(id);
    }

    public SellerProfile? GetSellerProfileByIdAllWithCollections(string id)
    {
        throw new NotImplementedException();
    }
}