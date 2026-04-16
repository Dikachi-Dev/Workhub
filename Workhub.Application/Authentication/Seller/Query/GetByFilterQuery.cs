using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Query;

internal record GetByFilterQuery(string Filter, int pageNumber, int pageSize) : IRequest<ErrorOr<GetFilterResult>>;

