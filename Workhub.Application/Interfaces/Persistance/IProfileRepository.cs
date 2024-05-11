using System.Security.Claims;
using Workhub.Domain.Dtos;
using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;
public interface IProfileRepository : IGenericRepository<Profile>
{
    Profile? GetProfileByEmail(string email);
    IEnumerable<ProfileResponse?> GetByFilter(string filter, int pageNumber, int pageSize);
    Profile? GetSellerProfileByIdAllWithCollections(string id);
    IQueryable<Profile?> GetQueryableSellerProfiles();
    Profile GetVendor(string id);
    Task<bool> DeleteUser(string id);
    Task<bool> ChangePass(string password, string id, string oldpass);
    Task<bool> UserExista(string email, string phonenumber);
    Task<bool> ResetPassword(string email, string token, string newpassword);
    Task<bool> ResetPassCode(string email);
    IQueryable<ProfileResponse?> GetByProximity();
    IEnumerable<ProfileResponse> GetAllVendors(int pageNumber, int pageSize);
    Task<IEnumerable<ProfileResponse>> GetByOccupation(string occupation, string country);
    Task<IEnumerable<ProfileResponse>> GetAllVendros(string country);
    Task<IList<Claim>> Login(string username, string password, string token);
    Task<GlobalUser> Register(Profile profile);
    Task<SubResult> IsSubscribed(string userId);
    Task<string> Subscribed(string userId);
    Task<bool> isVerified(string userId);
    bool isSubActive();
}

