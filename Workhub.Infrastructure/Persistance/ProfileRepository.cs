using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
{
    public ProfileRepository(AppDataContext context) : base(context)
    {
    }

    public IQueryable<Profile?> GetByFilter(string filter)
    {
        return GetAll()
           .Where(profile => profile != null && profile.FirstName
           .Contains(filter) || profile.Email
           .Contains(filter) || profile.LastName
           .Contains(filter) || profile.State
           .Contains(filter) || profile.Country
           .Contains(filter));
    }

    public IQueryable<Profile?> GetQueryableSellerProfiles()
    {
        throw new NotImplementedException();
    }

    public Profile? GetProfileByEmail(string email)
    {
        return GetAll().FirstOrDefault(x => x.Email == email);
    }

    public Profile? GetProfileById(string id)
    {
        return GetById(id);
    }

    public Profile? GetSellerProfileByIdAllWithCollections(string id)
    {
        throw new NotImplementedException();
    }
}