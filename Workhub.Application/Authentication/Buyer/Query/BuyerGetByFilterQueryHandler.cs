using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Query
{
    internal class BuyerGetByFilterQueryHandler : IRequestHandler<BuyerGetByFilterQuery, ErrorOr<BuyerFilterResult>>
    {
        private readonly IBuyerProfileRepository repository;

        public BuyerGetByFilterQueryHandler(IBuyerProfileRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ErrorOr<BuyerFilterResult>> Handle(BuyerGetByFilterQuery request, CancellationToken cancellationToken)
        {
            if (repository.GetBuyerFilter(request.Filter) is not IEnumerable<BuyerProfile> profiles)
            {
                return Domain.Errors.BuyerProfile.NotFound;
            }

            return new BuyerFilterResult(profiles);
        }
    }
}
