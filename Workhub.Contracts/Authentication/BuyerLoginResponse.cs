using Workhub.Domain.Entities;

namespace Workhub.Contracts.Authentication;

public record BuyerLoginResponse(BuyerProfile BuyerProfile, string Token);
