using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Common.Models;

namespace Workhub.Application.Profiless.Query;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ErrorOr<GetAllResult>>
{
    private readonly IMediator mediator;
    private readonly IProfileRepository repository;


    public GetAllQueryHandler(IMediator mediator, IProfileRepository repository)
    {
        this.mediator = mediator;
        this.repository = repository;

    }

    public async Task<ErrorOr<GetAllResult>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        if (request.Filter == null)
        {
            IEnumerable<ProfileResponse> profiles = repository.GetAllVendors(request.pageNumber, request.pageSize);
            var myProfileResults = profiles.Select(p => new MyProfileResult(
                FirstName: p.FirstName,
                LastName: p.LastName,
                PhoneNumber: p.PhoneNumber,
                ProfileImage: p.ProfileImage,
                Country: p.Country,
                Address: p.Address,
                State: p.State,
                Occupation: p.Occupation,
                Experience: p.Experience,
                Rating: p.Rating,
                Id: p.Id
            ));
            return new GetAllResult(myProfileResults);

        }
        else
        {
            if (repository.GetByFilter(request.Filter, request.pageNumber, request.pageSize) is not IEnumerable<ProfileResponse> profiles)
            {
                return Domain.Errors.Errors.Profile.NotFound;
            }
            var myProfileResults = profiles.Select(p => new MyProfileResult(
                FirstName: p.FirstName,
                LastName: p.LastName,
                PhoneNumber: p.PhoneNumber,
                ProfileImage: p.ProfileImage,
                Country: p.Country,
                Address: p.Address,
                State: p.State,
                Occupation: p.Occupation,
                Experience: p.Experience,
                Rating: p.Rating,
                Id: p.Id
            )).Take(100);
            return new GetAllResult(myProfileResults);
        }
    }
}
