using ErrorOr;
using MediatR;
using Workhub.Application.Common.Helpers;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Common.Models;

namespace Workhub.Application.Profiless.Query
{
    public class GetByProxQueryHandler : IRequestHandler<GetByProxQuery, ErrorOr<ProxyResult>>
    {
        private readonly IJobRepository jobRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IMediator mediator;
        private readonly ICloseProx closeProx;

        public GetByProxQueryHandler(IJobRepository jobRepository, IProfileRepository profileRepository, IMediator mediator, ICloseProx closeProx)
        {
            this.jobRepository = jobRepository;
            this.profileRepository = profileRepository;
            this.mediator = mediator;
            this.closeProx = closeProx;
        }

        public async Task<ErrorOr<ProxyResult>> Handle(GetByProxQuery request, CancellationToken cancellationToken)
        {
            var profile = await profileRepository.GetById(request.UserId);
            if (profile == null)
            {
                return Domain.Errors.Errors.Profile.NotFound;
            }

            var userLocation = profile.Location ?? GeospatialHelper.ParseLongLat(profile.LongLat);
            if (userLocation == null)
            {
                // Fallback to occupation search if user location is not set
                var fallbackProfiles = await profileRepository.GetByOccupation(request.Occupation, profile.Country);
                if (!fallbackProfiles.Any())
                {
                    return new ProxyResult([]);
                }

                var fallbackResults = fallbackProfiles.Select(p => new MyProfileResult(
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

            // Use PostGIS spatial query for proximity search filtered by occupation
            var closeProximity = await profileRepository.GetProfilesByProximity(
                userLocation, 
                profile.Country, 
                occupation: request.Occupation,
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
}
