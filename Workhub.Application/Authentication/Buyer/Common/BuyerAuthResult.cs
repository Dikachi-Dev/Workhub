using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Common;

public record BuyerAuthResult(
    BuyerProfile buyerProfile,
    string token);

