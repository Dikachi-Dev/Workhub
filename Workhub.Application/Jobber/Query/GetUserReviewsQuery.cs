using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;

namespace Workhub.Application.Jobber.Query;

public record ReviewDto(string Name, int Rating, string Remark);

public record GetUserReviewsQuery(string UserId) : IRequest<ErrorOr<List<ReviewDto>>>;

public class GetUserReviewsQueryHandler : IRequestHandler<GetUserReviewsQuery, ErrorOr<List<ReviewDto>>>
{
    private readonly IJobRepository jobRepository;

    public GetUserReviewsQueryHandler(IJobRepository jobRepository)
    {
        this.jobRepository = jobRepository;
    }

    public async Task<ErrorOr<List<ReviewDto>>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await jobRepository.GetUserJobs(request.UserId);
        var reviews = jobs
            .Where(j => j.IsRated)
            .Select(j => new ReviewDto(j.BuyerName, j.SellerRating, j.Remark))
            .ToList();

        return reviews;
    }
}
