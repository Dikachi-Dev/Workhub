using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.ChatAp.Query;
using Workhub.Contracts.Chat;

namespace Workhub.Api.EndPoints.ChatEndpoints;

public static class GetAllChatEndpoint
{
    public static void MappAllchatEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/allchats", async (IMediator mediator, IMapper mapper, ChatByUserIdRequest request) =>
        {
            var query = mapper.Map<ChatByUserIdQuery>(request);
            ErrorOr<AllChatResult> allResult = await mediator.Send(query);
            return allResult.Match(allresult => Results.Ok(mapper.Map<ChatByUserIdResponse>(allresult)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });
    }
}
