using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;
public interface IProfileRepository : IGenericRepository<Profile>
{
    Profile? GetProfileByEmail(string email, CancellationToken token);
    IQueryable<Profile?> GetByFilter(string filter, CancellationToken token);
    Profile? GetSellerProfileByIdAllWithCollections(string id, CancellationToken token);
    IQueryable<Profile?> GetQueryableSellerProfiles(CancellationToken token);
}

