using Microsoft.EntityFrameworkCore;
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
        job.Accept();
        await SaveChanges();
        return job;
    }

    public async void Cancel(string jobId)
    {
        var job = await GetById(jobId);
        job.Cancel();
        await SaveChanges();
    }

    public async Task<Job> Remark(string jobId, int rating, string remark)
    {
        var job = await GetById(jobId);
        job.Rate(rating, remark);
        await SaveChanges();
        return job;
    }

    public async Task<Job> Decline(string jobId)
    {
        var job = await GetById(jobId);
        job.Status = "Declined";
        await SaveChanges();
        return job;
    }

    public async Task<IList<Job>> GetUserJobs(string userId)
    {
        return await DbSet
            .Where(p => p.BuyerId == userId || p.SellerId == userId)
            .OrderByDescending(o => o.CreatedOn)
            .ToListAsync();
    }

    public async Task<IList<Job>> GetSellerJobs(string userId)
    {
        return await DbSet
            .Where(p => p.SellerId == userId)
            .OrderByDescending(o => o.CreatedOn)
            .ToListAsync();
    }
}
