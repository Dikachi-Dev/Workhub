using ErrorOr;
using MediatR;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Application.Jobber.Query;

public record GetUserJobsQuery(string UserId) : IRequest<ErrorOr<IList<Job>>>;

public class GetUserJobsQueryHandler : IRequestHandler<GetUserJobsQuery, ErrorOr<IList<Job>>>
{
    private readonly IJobRepository jobRepository;

    public GetUserJobsQueryHandler(IJobRepository jobRepository)
    {
        this.jobRepository = jobRepository;
    }

    public async Task<ErrorOr<IList<Job>>> Handle(GetUserJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await jobRepository.GetUserJobs(request.UserId);
        return (List<Job>)jobs;
    }
}
