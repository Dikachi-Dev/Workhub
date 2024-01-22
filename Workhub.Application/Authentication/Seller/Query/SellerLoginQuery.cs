using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Query;
public record SellerLoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<SellerAuthResult>>;

