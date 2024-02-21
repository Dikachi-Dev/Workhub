using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.ChatAp.Query;
using Workhub.Contracts.Chat;

namespace Workhub.Api.EndPoints.ChatEndpoints;

public static class GetBidirectionChatEndpoint
{
    public static void MapGetBidirectionChatEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/end2end", async (IMediator mediator, IMapper mapper, ChatBidirectionalRequest request) =>
        {
            var query = mapper.Map<ChatBidirectionalQuery>(request);
            ErrorOr<ChatResult> chatResult = await mediator.Send(query);
            return chatResult.Match(chat => Results.Ok(mapper.Map<ChatBidirectionalResponse>(chat)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });
    }
}
