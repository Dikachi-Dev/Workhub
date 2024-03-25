using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IJobRepository : IGenericRepository<Job>
{
    Task<Job> CreateJob(string userId, string occupation);
    Task<IEnumerable<Job>> GetUserJobs(string userId);
    Task<Job> Accept (string jobId);
    void Decline (string jobId);
    void Cancel (string jobId);
}
