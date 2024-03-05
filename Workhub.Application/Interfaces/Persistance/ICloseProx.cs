using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance
{
    public interface ICloseProx
    {
        Task<List<Profile>> GetProfilesSortedByProximity(string origin, string destinations, IEnumerable<Profile> profiles);
    }
}
