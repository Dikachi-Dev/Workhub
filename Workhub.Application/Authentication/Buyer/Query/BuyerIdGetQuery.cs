using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;

namespace Workhub.Application.Authentication.Buyer.Query;

internal record BuyerIdGetQuery(string userid) : IRequest<ErrorOr<BuyerGetResult>>;

