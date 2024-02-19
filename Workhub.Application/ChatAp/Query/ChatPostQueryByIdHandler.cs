using ErrorOr;
using MediatR;
using Workhub.Application.ChatAp.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Query;

public class ChatPostQueryByIdHandler : IRequestHandler<ChatPostQueryById, ErrorOr<ChatResult>>
{
    private readonly IChatPostRepository repository;
    private readonly IMediator mediator;

    public ChatPostQueryByIdHandler(IChatPostRepository repository, IMediator mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<ErrorOr<ChatResult>> Handle(ChatPostQueryById request, CancellationToken cancellationToken)
    {
        if (await repository.GetById(request.Id) is not ChatPost chat)
        {
            return Domain.Errors.Errors.ChatPost.NotFound;
        }
        return new ChatResult(chat);
    }
}
