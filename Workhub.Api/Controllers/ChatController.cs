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
    [Authorize(Roles = "Both,Seller,User")]
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
        public async Task<IResult> End2End([FromQuery] string receiverId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = new ChatBidirectionalQuery(userId.Trim(), receiverId.Trim());
            ErrorOr<ChatResult> chatResult = await mediator.Send(query);
            return chatResult.Match(chat =>
            Results.Ok(mapper.Map<ChatBidirectionalResponse>(chat)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }

        [HttpPost("send")]
        public async Task<IResult> Send(ChatRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new CreateChatCommand(userId, request.ReceiverId, request.Message);
            ErrorOr<ChatResult> chat = await mediator.Send(command);
            return chat.Match(chatresult =>
            Results.Ok(mapper.Map<ChatResult>(chatresult)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }
        [HttpGet("allchats")]
        public async Task<IResult> AllChat()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new ChatByUserIdQuery(userId.Trim());
            ErrorOr<AllChatResult> allResult = await mediator.Send(query);
            return allResult.Match(allresult =>
            Results.Ok(mapper.Map<ChatByUserIdResponse>(allresult)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }

    }
}
