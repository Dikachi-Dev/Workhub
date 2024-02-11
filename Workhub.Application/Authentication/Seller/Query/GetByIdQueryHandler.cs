using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query
{
    internal class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ErrorOr<GetResult>>
    {
        private readonly IProfileRepository repository;

        public GetByIdQueryHandler(IProfileRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ErrorOr<GetResult>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            if (await repository.GetById(request.Userid) is not Profile profile)
            {
                return Domain.Errors.Errors.Profile.NotFound;
            }
            return new GetResult(profile);
        }
    }
}
