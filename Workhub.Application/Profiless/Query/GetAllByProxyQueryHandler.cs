using ErrorOr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Profiless.Common;
using Workhub.Domain.Dtos;

namespace Workhub.Application.Profiless.Query;

public class GetAllByProxyQueryHandler : IRequestHandler<GetAllByProxyQuery, ErrorOr<ProxyResult>>
{
    private readonly IMediator mediator;
    private readonly IProfileRepository repository;
    private readonly ICloseProx closeProx;


    public GetAllByProxyQueryHandler(IMediator mediator, IProfileRepository repository, ICloseProx closeProx)
    {
        this.mediator = mediator;
        this.repository = repository;
        this.closeProx = closeProx;
    }

    public async Task<ErrorOr<ProxyResult>> Handle(GetAllByProxyQuery request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetById(request.userid);
        var profiles = await repository.GetAllVendros(profile.Country);

        if (profiles.IsNullOrEmpty())
        {
            return new ProxyResult([]);
        }
        string destinations = string.Join("|", profiles.Select(p => p.LongLat));
        string origin = profile.LongLat;
        List<ProfileResponse> closeProximity = await closeProx.GetProfilesSortedByProximity(origin, destinations, profiles);
        var myProfileResults = new ProxyResult(closeProximity.Select(p => new MyProfileResult(
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
        )).ToList());

        return myProfileResults;
    }
}
