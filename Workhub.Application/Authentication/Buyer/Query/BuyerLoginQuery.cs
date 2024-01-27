using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;

namespace Workhub.Application.Authentication.Buyer.Query;

public record BuyerLoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<BuyerAuthResult>>;

