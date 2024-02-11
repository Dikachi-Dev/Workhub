using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query;

internal class GetByFilterQueryHandler : IRequestHandler<GetByFilterQuery, ErrorOr<GetFilterResult>>
{
    private readonly IProfileRepository repository;

    public GetByFilterQueryHandler(IProfileRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ErrorOr<GetFilterResult>> Handle(GetByFilterQuery request, CancellationToken cancellationToken)
    {
        if (repository.GetByFilter(request.Filter) is not IEnumerable<Profile> profiles)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        return new GetFilterResult(profiles);
    }
}
