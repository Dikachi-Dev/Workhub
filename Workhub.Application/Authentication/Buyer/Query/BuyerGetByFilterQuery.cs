using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;


namespace Workhub.Application.Authentication.Buyer.Query;

internal record BuyerGetByFilterQuery(string Filter) : IRequest<ErrorOr<BuyerFilterResult>>;
