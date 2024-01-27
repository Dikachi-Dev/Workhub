using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query;

internal class SellerGetByFilterQueryHandler : IRequestHandler<SellerGetByFilterQuery, ErrorOr<SellerGetFilterResult>>
{
    private readonly ISellerProfileRepository repository;

    public SellerGetByFilterQueryHandler(ISellerProfileRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ErrorOr<SellerGetFilterResult>> Handle(SellerGetByFilterQuery request, CancellationToken cancellationToken)
    {
        if (repository.GetSellerFilter(request.Filter) is not IEnumerable<SellerProfile> profiles)
        {
            return Domain.Errors.SellerProfile.NotFound;
        }

        return new SellerGetFilterResult(profiles);
    }
}
