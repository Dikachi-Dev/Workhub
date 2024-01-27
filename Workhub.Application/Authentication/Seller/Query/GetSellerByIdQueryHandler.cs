using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query
{
    internal class GetSellerByIdQueryHandler : IRequestHandler<GetSellerByIdquery, ErrorOr<SellerGetResult>>
    {
        private readonly ISellerProfileRepository repository;

        public GetSellerByIdQueryHandler(ISellerProfileRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ErrorOr<SellerGetResult>> Handle(GetSellerByIdquery request, CancellationToken cancellationToken)
        {
            if (repository.GetSellerProfileById(request.Userid) is not SellerProfile profile)
            {
                return Domain.Errors.SellerProfile.NotFound;
            }
            return new SellerGetResult(profile);
        }
    }
}
