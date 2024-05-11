using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Profiless.Common;

namespace Workhub.Application.Profiless.Query;

public class MyProfileQueryHandler : IRequestHandler<MyProfileQuery, ErrorOr<MyProfileResult>>
{
    private readonly IProfileRepository profileRepository;
    private readonly IMediator mediator;

    public MyProfileQueryHandler(IProfileRepository profileRepository, IMediator mediator)
    {
        this.profileRepository = profileRepository;
        this.mediator = mediator;

    }

    public async Task<ErrorOr<MyProfileResult>> Handle(MyProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.GetById(request.userId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }
        return new MyProfileResult(FirstName: profile.FirstName,
            LastName: profile.LastName,
            PhoneNumber: profile.PhoneNumber,
            ProfileImage: profile.ProfileImage,
            Country: profile.Country,
            Address: profile.Address,
            State: profile.State,
            Occupation: profile.Occupation,
            Experience: profile.Experience,
            Rating: profile.Rating,
            Id: profile.Id);
    }
}
