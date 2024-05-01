using System.Security.Claims;
using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;
public interface IProfileRepository : IGenericRepository<Profile>
{
    Profile? GetProfileByEmail(string email);
    IQueryable<Profile?> GetByFilter(string filter);
    Profile? GetSellerProfileByIdAllWithCollections(string id);
    IQueryable<Profile?> GetQueryableSellerProfiles();
    Profile GetVendor(string id);
    bool DeleteUser(string id);
    bool ChangePass(string password, string id, string oldpass);
    Task<bool> ResetPassword(string email, string token, string newpassword);
    Task<bool> ResetPassCode(string email);
    IQueryable<Profile?> GetByProximity();
    Task<IEnumerable<Profile>> GetByOccupation(string occupation);
    Task<IEnumerable<Profile>> GetAllVendros();
    Task<IList<Claim>> Login(string username, string password, string token);
    Task<GlobalUser> Register(Profile profile);
    Task<bool> IsSubscribed(string userId);
    Task<bool> Subscribed(string userId);
}

