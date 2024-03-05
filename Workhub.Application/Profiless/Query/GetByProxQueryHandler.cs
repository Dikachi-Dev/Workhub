using ErrorOr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Profiless.Common;
using Workhub.Domain.Entities;

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
            var profiles = await profileRepository.GetByOccupation(request.Occupation);
            var profile = await profileRepository.GetById(request.UserId);

            if (profiles.IsNullOrEmpty())
            {
                return new ProxyResult([]);
            }
            string destinations = string.Join("|", profiles.Select(p => p.LongLat));
            string origin = profile.LongLat;
            List<Profile> closeProximity = await closeProx.GetProfilesSortedByProximity(origin, destinations, profiles);
            return new ProxyResult(closeProximity);
        }
    }
}
