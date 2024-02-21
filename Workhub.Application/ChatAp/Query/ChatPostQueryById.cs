using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;

namespace Workhub.Application.ChatAp.Query;

public record ChatPostQueryById(string Id) : IRequest<ErrorOr<ChatResult>>;
