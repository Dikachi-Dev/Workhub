using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Command;

public class CreateChatcommandHandler : IRequestHandler<CreateChatCommand, ErrorOr<ChatResult>>
{
    private readonly IChatPostRepository repository;
    private readonly IMediator mediator;
    private readonly IProfileRepository profile;
    private readonly INotificationSender notification;

    public CreateChatcommandHandler(IChatPostRepository repository, IMediator mediator, INotificationSender notification, IProfileRepository profile)
    {
        this.repository = repository;
        this.mediator = mediator;
        this.notification = notification;
        this.profile = profile;
    }

    public async Task<ErrorOr<ChatResult>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {

        var existingChat = await repository.GetbySenderAndReciverId(request.SenderId, request.ReceiverId);
        var getter = await profile.GetById(request.ReceiverId);
        if (existingChat is null)
        {
            var chat = new ChatPost
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Replys = new List<Reply> { new Reply { Message = request.Message, FromId = request.SenderId } }
            };
            await repository.Add(chat);
            await notification.SendFcmMessage(getter.Token, "New Message",chat.Id, "newmessage");
        }
        else
        {
            var updatechat = existingChat.Replys.ToList();
            updatechat.Add(new Reply { Message = request.Message, FromId = request.SenderId });
            repository.Update(existingChat);
            await notification.SendFcmMessage(getter.Token, "New Message",existingChat.Id, "newmessage");
        }
       
        await repository.SaveChanges();

        return new ChatResult(await repository.GetbySenderAndReciverId(request.SenderId, request.ReceiverId));
    }
}
