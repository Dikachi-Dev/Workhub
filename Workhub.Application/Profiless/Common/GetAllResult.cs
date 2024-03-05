using Workhub.Domain.Entities;

namespace Workhub.Application.Profiless.Common;

public record GetAllResult(IEnumerable<Profile> Profiles);
