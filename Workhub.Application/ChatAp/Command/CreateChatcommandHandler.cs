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
        var getter2 = await profile.GetById(request.SenderId);
        if (existingChat is null)
        {
            var newchat = new ChatPost
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                SenderName = $"{getter2.FirstName} {getter2.LastName}",
                ReceiverName = $"{getter.FirstName} {getter.LastName}",
                Replys = new List<Reply> { new Reply { Message = request.Message, FromId = request.SenderId } }
            };
            await repository.Add(newchat);
            await repository.SaveChanges();
            await notification.SendFcmMessage(getter.Token, "New Message", newchat.Id, "newmessage", $"Message from {newchat.SenderName}");
        }
        else
        {
            existingChat.UpdatedOn = DateTime.UtcNow;
            existingChat.Replys.Add(new Reply { Message = request.Message, FromId = request.SenderId });
            await repository.SaveChanges(); // Save changes to the existing chat
            var test = existingChat;
            await notification.SendFcmMessage(getter.Token, "New Message", existingChat.Id, "newmessage", $"Message from {existingChat.SenderName}");
        }

        // Get the updated chat from the repository and return it
        var updatedChat = await repository.GetbySenderAndReciverId(request.SenderId, request.ReceiverId);
        return new ChatResult(updatedChat.SenderId, updatedChat.Id, updatedChat.CreatedOn, updatedChat.ReceiverId, updatedChat.ReceiverName, updatedChat.SenderName, updatedChat.Replys.Select(s => new Replyy(s.Id, s.CreatedOn, s.Message, s.FromId)).ToList());

    }
}
