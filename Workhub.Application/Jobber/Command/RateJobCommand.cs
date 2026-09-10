using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Jobber.Command;

public record RateJobCommand(
    string JobId,
    int Rating,
    string Remark) : IRequest<ErrorOr<Success>>;

public class RateJobCommandHandler : IRequestHandler<RateJobCommand, ErrorOr<Success>>
{
    private readonly IJobRepository jobRepository;
    private readonly IProfileRepository profileRepository;

    public RateJobCommandHandler(IJobRepository jobRepository, IProfileRepository profileRepository)
    {
        this.jobRepository = jobRepository;
        this.profileRepository = profileRepository;
    }

    public async Task<ErrorOr<Success>> Handle(RateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await jobRepository.GetById(request.JobId);
        if (job is null)
        {
            return Domain.Errors.Errors.Job.NotFound;
        }

        job.Rate(request.Rating, request.Remark);
        jobRepository.Update(job);
        await jobRepository.SaveChanges();

        // Recalculate seller's aggregate rating
        var sellerJobs = await jobRepository.GetSellerJobs(job.SellerId);
        var ratedJobs = sellerJobs.Where(j => j.IsRated).ToList();
        if (ratedJobs.Count > 0)
        {
            double averageRating = ratedJobs.Average(j => j.SellerRating);
            int roundedAverageRating = (int)Math.Round(averageRating, MidpointRounding.AwayFromZero);

            var sellerProfile = await profileRepository.GetById(job.SellerId);
            if (sellerProfile != null)
            {
                sellerProfile.UpdateRating(roundedAverageRating);
                profileRepository.Update(sellerProfile);
                await profileRepository.SaveChanges();
            }
        }

        return Result.Success;
    }
}
