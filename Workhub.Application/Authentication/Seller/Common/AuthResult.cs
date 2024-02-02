using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Common;

public record AuthResult(Profile Profile, string Token);

