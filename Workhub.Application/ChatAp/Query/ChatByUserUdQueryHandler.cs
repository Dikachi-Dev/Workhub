using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Query;

public class ChatByUserUdQueryHandler : IRequestHandler<ChatByUserIdQuery, ErrorOr<AllChatResult>>
{
    private readonly IChatPostRepository repository;
    private readonly IMediator mediator;

    public ChatByUserUdQueryHandler(IChatPostRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<ErrorOr<AllChatResult>> Handle(ChatByUserIdQuery request, CancellationToken cancellationToken)
    {
        //var chatposts = repository.GetByUser
        if (await repository.GetByUser(request.userId) is not IList<ChatPost> chatposts)
        {
            return Domain.Errors.Errors.ChatPost.NotFound;
        }
        Console.WriteLine(chatposts);
        // var chats = chatposts.ToList();
        var chats = new AllChatResult(
            chatposts.Select(c => new ChatResult(

                SenderId: c.SenderId,
                Id: c.Id,
                CreatedOn: c.CreatedOn,
                ReceiverId: c.ReceiverId,
                ReceiverName: c.ReceiverName,
                SenderName: c.SenderName,
                Replys: c.Replys.Select(r => new Replyy(
                     Id: r.Id,
                    CreatedOn: r.CreatedOn,
                    Message: r.Message,
                    FromId: r.FromId)).ToList())).ToList());


        return chats;

    }
}
