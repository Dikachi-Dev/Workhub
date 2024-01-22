using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Common;

internal record BuyerFilterResult(IEnumerable<BuyerProfile> profile);
