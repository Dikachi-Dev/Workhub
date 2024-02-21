using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;

namespace Workhub.Application.ChatAp.Query;

public record ChatByUserIdQuery(string userId) : IRequest<ErrorOr<AllChatResult>>;