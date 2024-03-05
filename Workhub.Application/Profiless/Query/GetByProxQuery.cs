using ErrorOr;
using MediatR;
using Workhub.Application.Profiless.Common;

namespace Workhub.Application.Profiless.Query;

public record GetByProxQuery(string UserId, string Occupation) : IRequest<ErrorOr<ProxyResult>>;

