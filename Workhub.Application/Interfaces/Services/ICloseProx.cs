using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Services;
public interface ICloseProx
{
    Task<List<Profile>> GetProfilesSortedByProximity(string origin, string destinations, IEnumerable<Profile> profiles);
    Task<dynamic> GetFullAddress(string longlat);
}