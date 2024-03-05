using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.ChatAp.Command;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.ChatAp.Query;
using Workhub.Contracts.Chat;

namespace Workhub.Api.EndPoints.ChatEndpoints;

public static class ChatEndpoint
{
    public static void MapChatEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/send", async (IMediator mediator, IMapper mapper, ChatRequest request) =>
        {
            var command = mapper.Map<CreateChatCommand>(request);
            ErrorOr<ChatResult> chat = await mediator.Send(command);
            return chat.Match(chatresult =>
            Results.Ok(mapper.Map<ChatResult>(chatresult)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });

        endpoint.MapGet("/end2end", async (IMediator mediator, IMapper mapper, ChatByUserIdRequest request) =>
        {
            var query = mapper.Map<ChatBidirectionalQuery>(request);
            ErrorOr<ChatResult> chatResult = await mediator.Send(query);
            return chatResult.Match(chat =>
            Results.Ok(mapper.Map<ChatBidirectionalResponse>(chat)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });

        endpoint.MapGet("/allchats", async (IMediator mediator, IMapper mapper, ChatByUserIdRequest request) =>
        {
            var query = mapper.Map<ChatByUserIdQuery>(request);
            ErrorOr<AllChatResult> allResult = await mediator.Send(query);
            return allResult.Match(allresult =>
            Results.Ok(mapper.Map<ChatByUserIdResponse>(allresult)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });
    }
}