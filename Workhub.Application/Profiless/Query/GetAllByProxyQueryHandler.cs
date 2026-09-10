using ErrorOr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Common.Models;
using Workhub.Application.Common.Helpers;

namespace Workhub.Application.Profiless.Query;

public class GetAllByProxyQueryHandler : IRequestHandler<GetAllByProxyQuery, ErrorOr<ProxyResult>>
{
    private readonly IMediator mediator;
    private readonly IProfileRepository repository;

    public GetAllByProxyQueryHandler(IMediator mediator, IProfileRepository repository)
    {
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task<ErrorOr<ProxyResult>> Handle(GetAllByProxyQuery request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetById(request.userid);
        
        // Parse user location from LongLat string
        var userLocation = GeospatialHelper.ParseLongLat(profile.LongLat);
        if (userLocation == null)
        {
            // Fallback to old method if location is invalid
            var profiles = await repository.GetAllVendros(profile.Country);
            if (!profiles.Any())
            {
                return new ProxyResult([]);
            }
            
            var fallbackResults = profiles.Select(p => new MyProfileResult(
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
            )).ToList();
            
            return new ProxyResult(fallbackResults);
        }
        
        // Use PostGIS spatial query for proximity search
        var closeProximity = await repository.GetProfilesByProximity(
            userLocation, 
            profile.Country, 
            radiusMeters: 50000, 
            limit: 100);
        
        if (!closeProximity.Any())
        {
            return new ProxyResult([]);
        }
        
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
