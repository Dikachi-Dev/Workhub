using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Command;

public class CreateChatcommandHandler : IRequestHandler<CreateChatCommand, ErrorOr<ChatResult>>
{
    private readonly IChatPostRepository repository;
    private readonly IMediator mediator;

    public CreateChatcommandHandler(IChatPostRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<ErrorOr<ChatResult>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new ChatPost
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Replys = new List<Reply> { new Reply { Message = request.Message, FromId = request.SenderId } }
        };
        await repository.Add(chat, cancellationToken);
        await repository.SaveChanges(cancellationToken);
        return new ChatResult(await repository.GetbySenderAndReciverId(request.SenderId, request.ReceiverId, cancellationToken));

    }
}
