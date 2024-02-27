using Workhub.Domain.Entities;

namespace Workhub.Contracts.Profileing;

public record GetAllResponse(IEnumerable<Profile> Profiles);
