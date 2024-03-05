using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Workhub.Api.EndPoints;
using Workhub.Application.ChatAp.Command;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.ChatAp.Query;
using Workhub.Contracts.Chat;

namespace Workhub.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IResult> End2End([FromQuery] string senderId, [FromQuery] string receiverId)
        {
            var query = new ChatBidirectionalQuery(senderId.Trim(), receiverId.Trim());
            ErrorOr<ChatResult> chatResult = await mediator.Send(query);
            return chatResult.Match(chat =>
            Results.Ok(mapper.Map<ChatBidirectionalResponse>(chat)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }

        [HttpPost("send")]
        public async Task<IResult> Send(ChatRequest request)
        {
            var command = mapper.Map<CreateChatCommand>(request);
            ErrorOr<ChatResult> chat = await mediator.Send(command);
            return chat.Match(chatresult =>
            Results.Ok(mapper.Map<ChatResult>(chatresult)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }
        [HttpGet("allchats")]
        public async Task<IResult> AllChat([FromQuery] string userId)
        {
            var query = new ChatByUserIdQuery(userId.Trim());
            ErrorOr<AllChatResult> allResult = await mediator.Send(query);
            return allResult.Match(allresult =>
            Results.Ok(mapper.Map<ChatByUserIdResponse>(allresult)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }

    }
}
