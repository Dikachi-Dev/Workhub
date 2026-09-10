using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Commands;

public record SubscribeCommand(string UserId) : IRequest<ErrorOr<string>>;

public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, ErrorOr<string>>
{
    private readonly IProfileRepository _repository;

    public SubscribeCommandHandler(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<string>> Handle(SubscribeCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Subscribed(request.UserId);
        return result;
    }
}
