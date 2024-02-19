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
        if (repository.GetByUser(request.userId) is not IEnumerable<ChatPost> chatposts)
        {
            return Domain.Errors.Errors.ChatPost.NotFound;
        }
        return new AllChatResult(chatposts);
    }
}
