using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;

namespace Workhub.Application.ChatAp.Command;

public record CreateChatCommand(string SenderId, string ReceiverId, string Message) : IRequest<ErrorOr<ChatResult>>;
