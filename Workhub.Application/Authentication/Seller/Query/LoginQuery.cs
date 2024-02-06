using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Query;
public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthResult>>;

