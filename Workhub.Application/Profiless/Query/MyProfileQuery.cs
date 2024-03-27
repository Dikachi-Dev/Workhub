using ErrorOr;
using MediatR;
using Workhub.Application.Profiless.Common;

namespace Workhub.Application.Profiless.Query;

public record MyProfileQuery(string userId) : IRequest<ErrorOr<MyProfileResult>>;

