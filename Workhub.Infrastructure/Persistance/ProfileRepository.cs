using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
{
    public ProfileRepository(AppDataContext context, CancellationToken token) : base(context, token)
    {
    }

    public IQueryable<Profile?> GetByFilter(string filter, CancellationToken token)
    {
        return GetAll(token)
           .Where(profile => profile != null && profile.FirstName
           .Contains(filter) || profile.Email
           .Contains(filter) || profile.LastName
           .Contains(filter) || profile.State
           .Contains(filter) || profile.Country
           .Contains(filter));
    }

    public IQueryable<Profile?> GetQueryableSellerProfiles(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Profile? GetProfileByEmail(string email, CancellationToken token)
    {
        return GetAll(token).FirstOrDefault(x => x.Email == email);
    }


    public Profile? GetSellerProfileByIdAllWithCollections(string id, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}