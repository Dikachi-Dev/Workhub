using ErrorOr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Profiless.Common;
using Workhub.Domain.Dtos;

namespace Workhub.Application.Profiless.Query
{
    public class GetByProxQueryHandler : IRequestHandler<GetByProxQuery, ErrorOr<ProxyResult>>
    {
        private readonly IJobRepository jobRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IMediator mediator;
        private readonly ICloseProx closeProx;
        //private readonly IFileUpload upload;

        public GetByProxQueryHandler(IJobRepository jobRepository, IProfileRepository profileRepository, IMediator mediator, ICloseProx closeProx)
        {
            this.jobRepository = jobRepository;
            this.profileRepository = profileRepository;
            this.mediator = mediator;
            this.closeProx = closeProx;
            //this.upload = upload;
        }

        public async Task<ErrorOr<ProxyResult>> Handle(GetByProxQuery request, CancellationToken cancellationToken)
        {
            var profile = await profileRepository.GetById(request.UserId);
            var profiles = await profileRepository.GetByOccupation(request.Occupation, profile.Country);

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
}
