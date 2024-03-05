using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IJobRepository : IGenericRepository<Job>
{
    Task<Job> CreateJob(string userId, string occupation);
}
