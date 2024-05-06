using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Profiless.Common;
using Workhub.Domain.Entities;

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
            IEnumerable<Profile> profiles = repository.GetAllVendors(request.pageNumber, request.pageSize);
            var myProfileResults = profiles.Select(p => new MyProfileResult(
                FirstName: p.FirstName,
                LastName: p.LastName,
                Email: p.Email,
                PhoneNumber: p.PhoneNumber,
                ProfileImage: p.ProfileImage,
                Country: p.Country,
                Address: p.Address,
                State: p.State,
                Occupation: p.Occupation,
                Gender: p.Gender,
                Experience: p.Experience,
                Rating: p.Rating,
                JobCount: p.JobCount,
                Token: p.Token,
                Id: p.Id,
                LongLat: p.LongLat,
                UserType: p.UserType
            ));
            return new GetAllResult(myProfileResults);

        }
        else
        {
            if (repository.GetByFilter(request.Filter, request.pageNumber, request.pageSize) is not IEnumerable<Profile> profiles)
            {
                return Domain.Errors.Errors.Profile.NotFound;
            }
            var myProfileResults = profiles.Where(p => p.UserType is not "User" && p.VendorProfile.Image1 != "" && p.isDeleted != true).Select(p => new MyProfileResult(
                FirstName: p.FirstName,
                LastName: p.LastName,
                Email: p.Email,
                PhoneNumber: p.PhoneNumber,
                ProfileImage: p.ProfileImage,
                Country: p.Country,
                Address: p.Address,
                State: p.State,
                Occupation: p.Occupation,
                Gender: p.Gender,
                Experience: p.Experience,
                Rating: p.Rating,
                JobCount: p.JobCount,
                Token: p.Token,
                Id: p.Id,
                LongLat: p.LongLat,
                UserType: p.UserType
            )).Take(100);
            return new GetAllResult(myProfileResults);
        }
    }
}
