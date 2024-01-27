using Workhub.Domain.Entities;

namespace Workhub.Contracts.Authentication;

public record SellerLoginResponse(SellerProfile SellerProfile, string Token);

