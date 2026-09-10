using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Profiless.Query;

public record GetIsVerifiedQuery(string UserId) : IRequest<ErrorOr<bool>>;

public class GetIsVerifiedQueryHandler : IRequestHandler<GetIsVerifiedQuery, ErrorOr<bool>>
{
    private readonly IProfileRepository _repository;

    public GetIsVerifiedQueryHandler(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<bool>> Handle(GetIsVerifiedQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.isVerified(request.UserId);
        return result;
    }
}
