using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Query;

public record GetSellerByIdquery(string Userid) : IRequest<ErrorOr<SellerGetResult>>;

