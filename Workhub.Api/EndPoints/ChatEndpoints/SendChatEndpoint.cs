using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.ChatAp.Command;
using Workhub.Application.ChatAp.Common;
using Workhub.Contracts.Chat;

namespace Workhub.Api.EndPoints.ChatEndpoints
{
    public static class SendChatEndpoint
    {
        public static void MapSendChatEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/send", async (IMediator mediator, IMapper mapper, ChatRequest request) =>
            {
                var command = mapper.Map<CreateChatCommand>(request);
                ErrorOr<ChatResult> chat = await mediator.Send(command);
                return chat.Match(chatresult => Results.Ok(mapper.Map<ChatResult>(chatresult)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
            });
        }
    }
}
