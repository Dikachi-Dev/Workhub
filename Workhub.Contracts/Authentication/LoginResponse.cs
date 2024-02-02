using Workhub.Domain.Entities;

namespace Workhub.Contracts.Authentication;

public record LoginResponse(Profile Profile, string Token);

