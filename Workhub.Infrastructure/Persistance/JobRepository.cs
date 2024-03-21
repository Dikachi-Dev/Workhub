using System.Data.Entity;
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

    public async Task<IEnumerable<Job>> GetUserJobs(string userId)
    {
        var jobs = await DbSet.Where(p => p.BuyerId == userId || p.SellerId == userId).ToListAsync();
        return jobs;
    }
}
