using ErrorOr;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Jobber.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Jobber.Command;

public class AutoCreateCommandHandler : IRequestHandler<AutoCreateCommand, ErrorOr<GetJobResult>>
{
    private readonly IJobRepository jobRepository;
    private readonly IProfileRepository profileRepository;
    private readonly ICloseProx closeProx;
    private readonly INotificationSender sender;

    public AutoCreateCommandHandler(IJobRepository jobRepository, IProfileRepository profileRepository, ICloseProx closeProx, INotificationSender sender)
    {
        this.jobRepository = jobRepository;
        this.profileRepository = profileRepository;
        this.closeProx = closeProx;
        this.sender = sender;
    }

    public async Task<ErrorOr<GetJobResult>> Handle(AutoCreateCommand request, CancellationToken cancellationToken)
    {
        var profiles = await profileRepository.GetByOccupation(request.Occupation);
        var profile = await profileRepository.GetById(request.UserId);

        if (profiles.IsNullOrEmpty())
        {
            return new GetJobResult(new Job());
        }
        string destinations = string.Join("|", profiles.Select(p => p.LongLat));
        string origin = profile.LongLat;
        List<Profile> closeProximity = await closeProx.GetProfilesSortedByProximity(origin, destinations, profiles);

        if (closeProximity.Count > 0)
        {
            var choosen = closeProximity.First();
            var job = new Job
            {
                BuyerId = request.UserId,
                SellerId = choosen.Id,
                BuyerName = $"{profile.FirstName} {profile.LastName}",
                SellerName = $"{choosen.FirstName} {choosen.LastName}",
                Occupation = request.Occupation,
                Status = "Pending",
                Profile = profile
            };
            await jobRepository.Add(job);
            await jobRepository.SaveChanges();
            await sender.SendFcmMessage(choosen.Token,"New Job Alert",job.Id,"newjob");
            return new GetJobResult(job);
        }

        return new GetJobResult(new Job());

    }
}
