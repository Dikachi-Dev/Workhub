using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Common;

internal record GetFilterResult(IEnumerable<Profile> Profiles);
