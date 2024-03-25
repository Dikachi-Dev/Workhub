using System.Data.Entity;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class JobRepository : GenericRepository<Job>, IJobRepository
{
    public JobRepository(AppDataContext context) : base(context)
    {
    }

    public async Task<Job> Accept(string jobId)
    {
        var job = await GetById(jobId);
        job.Status = "Accepted";
        await SaveChanges();
        return job;
    }

    public async void Cancel(string jobId)
    {
        var job = await GetById(jobId);
        job.Status = "Cancelled";
        await SaveChanges();
    }

    public Task<Job> CreateJob(string userId, string occupation)
    {
        throw new NotImplementedException();
    }

    public async void Decline(string jobId)
    {
        var job = await GetById(jobId);
        job.Status = "Declined";
        await SaveChanges();
    }

    public async Task<IEnumerable<Job>> GetUserJobs(string userId)
    {
        var jobs = await DbSet.Where(p => p.BuyerId == userId || p.SellerId == userId).ToListAsync();
        return jobs;
    }
}
