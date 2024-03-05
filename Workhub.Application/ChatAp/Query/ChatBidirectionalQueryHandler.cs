using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Query;

public class ChatBidirectionalQueryHandler : IRequestHandler<ChatBidirectionalQuery, ErrorOr<ChatResult>>
{
    private readonly IChatPostRepository repository;
    private readonly IMediator mediator;

    public ChatBidirectionalQueryHandler(IChatPostRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<ErrorOr<ChatResult>> Handle(ChatBidirectionalQuery request, CancellationToken cancellationToken)
    {
        if (await repository.GetbySenderAndReciverId(request.senderId, request.receiverId) is not ChatPost post)
        {
            return Domain.Errors.Errors.ChatPost.NotFound;
        }
        return new ChatResult(post);
    }
}
