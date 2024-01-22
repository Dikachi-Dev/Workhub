using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Query;

internal class BuyerIdGetQueryHandler : IRequestHandler<BuyerIdGetQuery, ErrorOr<BuyerGetResult>>
{
    private readonly IBuyerProfileRepository repository;

    public BuyerIdGetQueryHandler(IBuyerProfileRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ErrorOr<BuyerGetResult>> Handle(BuyerIdGetQuery request, CancellationToken cancellationToken)
    {
        if (repository.GetBuyerProfileById(request.userid) is not BuyerProfile profile)
        {
            return Domain.Errors.BuyerProfile.NotFound;
        }

        return new BuyerGetResult(profile);
    }
}
