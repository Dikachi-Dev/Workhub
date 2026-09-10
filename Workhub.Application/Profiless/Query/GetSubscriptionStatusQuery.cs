using ErrorOr;
using MediatR;
using Workhub.Application.Common.Models;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Query;

public record GetSubscriptionStatusQuery(string UserId) : IRequest<ErrorOr<SubResult>>;

public class GetSubscriptionStatusQueryHandler : IRequestHandler<GetSubscriptionStatusQuery, ErrorOr<SubResult>>
{
    private readonly IProfileRepository _repository;

    public GetSubscriptionStatusQueryHandler(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<SubResult>> Handle(GetSubscriptionStatusQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.IsSubscribed(request.UserId);
        return result;
    }
}
