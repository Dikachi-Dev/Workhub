using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Query;

internal record GetByFilterQuery(string Filter) : IRequest<ErrorOr<GetFilterResult>>;

