using ErrorOr;
using MediatR;
using Workhub.Application.Jobber.Common;

namespace Workhub.Application.Jobber.Query;

internal record GetbyIdQuery(string Id) : IRequest<ErrorOr<GetResult>>;
