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

namespace Workhub.Api.Controllers
{
    [Authorize(Roles = "Both,Vendor,User")]
    [Route("api/chat")]
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
        public async Task<IResult> End2End([FromQuery] string receiverId, string senderId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || userId is "")
            {
                return Results.BadRequest("User Not Found");
            }
            var query = new ChatBidirectionalQuery(senderId.Trim(), receiverId.Trim());
            ErrorOr<ChatResult> chatResult = await mediator.Send(query);
            return chatResult.Match(chat =>
            Results.Ok(new ChatResponse(chat.SenderId, chat.Id, chat.CreatedOn, chat.ReceiverId, chat.ReceiverName, chat.SenderName, chat.Replys.Select(reply => new Replyyy(reply.Id, reply.CreatedOn, reply.Message, reply.FromId)).ToList())), errors =>
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
            Results.Ok(new ChatResponse(chat.SenderId, chat.Id, chat.CreatedOn, chat.ReceiverId, chat.ReceiverName, chat.SenderName, chat.Replys.Select(reply => new Replyyy(reply.Id, reply.CreatedOn, reply.Message, reply.FromId)).ToList())), errors =>
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
                var chats = allresult.ChatPosts.Select(c => new ChatResponse(
                    SenderId: c.SenderId,
                    Id: c.Id,
                    CreatedOn: c.CreatedOn,
                    ReceiverId: c.ReceiverId,
                    ReceiverName: c.ReceiverName,
                    SenderName: c.SenderName,
                    Replys: c.Replys.Select(r => new Replyyy(
                        Id: r.Id,
                        CreatedOn: r.CreatedOn,
                        Message: r.Message,
                        FromId: r.FromId)
                    )
                    .ToList()

                    )
                );
                return Results.Ok(chats);
            }, errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }

    }
}
