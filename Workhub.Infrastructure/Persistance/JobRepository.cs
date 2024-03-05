using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

internal class JobRepository : GenericRepository<Job>, IJobRepository
{
    public JobRepository(AppDataContext context) : base(context)
    {
    }

    public Task<Job> CreateJob(string userId, string occupation)
    {
        throw new NotImplementedException();
    }
}
