using ErrorOr;
using MediatR;
using Workhub.Application.Profiless.Common;

namespace Workhub.Application.Profiless.Query;

public record GetAllQuery(string? Filter) : IRequest<ErrorOr<GetAllResult>>;
