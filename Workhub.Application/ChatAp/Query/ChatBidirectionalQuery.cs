using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;

namespace Workhub.Application.ChatAp.Query;

public record ChatBidirectionalQuery(string senderId, string receiverId) : IRequest<ErrorOr<ChatResult>>;

