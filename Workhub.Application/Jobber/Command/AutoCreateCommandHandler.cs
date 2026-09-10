using ErrorOr;
using MediatR;
using Newtonsoft.Json;
using Workhub.Application.Common.Helpers;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Jobber.Common;
using Workhub.Application.Common.Models;
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
        var profile = await profileRepository.GetById(request.UserId);
        if (profile == null)
        {
            return new GetJobResult(new Job());
        }

        ProfileResponse? chosen = null;
        var userLocation = profile.Location ?? GeospatialHelper.ParseLongLat(profile.LongLat);

        if (userLocation != null)
        {
            // Use PostGIS spatial query for proximity search filtered by occupation
            var closeProfiles = await profileRepository.GetProfilesByProximity(
                userLocation, 
                profile.Country, 
                occupation: request.Occupation, 
                radiusMeters: 50000, 
                limit: 10);

            chosen = closeProfiles.FirstOrDefault();
        }

        // Fallback to general occupation search if no PostGIS match or location not available
        if (chosen == null)
        {
            var fallbackProfiles = await profileRepository.GetByOccupation(request.Occupation, profile.Country);
            chosen = fallbackProfiles.FirstOrDefault();
        }

        if (chosen != null)
        {
            var job = new Job
            {
                BuyerId = request.UserId,
                SellerId = chosen.Id,
                BuyerName = $"{profile.FirstName} {profile.LastName}",
                SellerName = $"{chosen.FirstName} {chosen.LastName}",
                Occupation = request.Occupation,
                Status = "Pending",
            };
            await jobRepository.Add(job);
            await jobRepository.SaveChanges();

            string body = JsonConvert.SerializeObject(new { jobId = job.Id, buyerName = job.BuyerName });
            await sender.SendFcmMessage(profile.Token, "New Job Alert", body, "newjob", $"You have new Hire Request from {job.BuyerName}");

            return new GetJobResult(job);
        }

        return new GetJobResult(new Job());
    }
}
