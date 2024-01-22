using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Common;

internal record SellerGetFilterResult(IEnumerable<SellerProfile> Profiles);
