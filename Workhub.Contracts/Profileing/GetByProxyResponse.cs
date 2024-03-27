using Workhub.Domain.Entities;

namespace Workhub.Contracts.Profileing;

internal record GetByProxyResponse(IList<Profile> Profiles);

