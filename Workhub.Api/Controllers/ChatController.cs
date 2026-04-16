using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workhub.Api.EndPoints;
using Workhub.Application.ChatAp.Command;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.ChatAp.Query;
using Workhub.Contracts.Chat;
using Asp.Versioning;

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Vendor,User")]
[Route("api/v{version:apiVersion}/chat")]
[ApiVersion("1.0")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public ChatController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpGet("end2end")]
    public async Task<IResult> End2End([FromQuery] string receiverId)
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        // Use logged-in user as sender
        var query = new ChatBidirectionalQuery(userId.Trim(), receiverId.Trim());
        ErrorOr<ChatResult> chatResult = await mediator.Send(query);
        return chatResult.Match(chat =>
        Results.Ok(MapToChatResponse(chat)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("send")]
    public async Task<IResult> Send(ChatRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var command = new CreateChatCommand(userId, request.ReceiverId, request.Message);
        ErrorOr<ChatResult> chat = await mediator.Send(command);
        return chat.Match(chat =>
        Results.Ok(MapToChatResponse(chat)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
    [HttpGet("allchats")]
    public async Task<IResult> AllChat()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var query = new ChatByUserIdQuery(userId.Trim());
        ErrorOr<AllChatResult> allResult = await mediator.Send(query);
        return allResult.Match(allresult =>
        {
            var chats = allresult.ChatPosts.Select(MapToChatResponse);
            return Results.Ok(chats);
        }, errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    private static ChatResponse MapToChatResponse(ChatResult chat)
    {
        return new ChatResponse(
            chat.SenderId,
            chat.Id,
            chat.CreatedOn,
            chat.ReceiverId,
            chat.ReceiverName,
            chat.SenderName,
            chat.Replys.Select(reply => new ReplyDto(reply.Id, reply.CreatedOn, reply.Message, reply.FromId)).ToList()
        );
    }

}
