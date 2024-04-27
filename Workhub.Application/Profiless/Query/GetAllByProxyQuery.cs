using ErrorOr;
using MediatR;
using Workhub.Application.Profiless.Common;

namespace Workhub.Application.Profiless.Query;

public record GetAllByProxyQuery(string userid) : IRequest<ErrorOr<ProxyResult>>;

