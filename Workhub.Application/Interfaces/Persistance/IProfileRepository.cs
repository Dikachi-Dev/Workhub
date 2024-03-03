using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;
public interface IProfileRepository : IGenericRepository<Profile>
{
    Profile? GetProfileByEmail(string email);
    IQueryable<Profile?> GetByFilter(string filter);
    Profile? GetSellerProfileByIdAllWithCollections(string id);
    IQueryable<Profile?> GetQueryableSellerProfiles();
    Task<GlobalUser> Login(string username, string password);
    Task<GlobalUser> Register(Profile profile);
}

