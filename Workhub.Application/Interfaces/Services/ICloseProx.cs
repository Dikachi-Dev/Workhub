using Workhub.Domain.Dtos;

namespace Workhub.Application.Interfaces.Services;
public interface ICloseProx
{
    Task<List<ProfileResponse>> GetProfilesSortedByProximity(string origin, string destinations, IEnumerable<ProfileResponse> profiles);
    Task<dynamic> GetFullAddress(string longlat);
}