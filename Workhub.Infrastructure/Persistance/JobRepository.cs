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
    public async Task<Job> Remark(string jobId, int rating, string remark)
    {
        var job = await GetById(jobId);
        job.SellerRating = rating;
        job.Remark = remark;
        job.Status = "Completed";
        job.IsRated = true;
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
        var query = DbSet
                     .Where(p => p.BuyerId == userId || p.SellerId == userId).OrderByDescending(o => o.CreatedOn);
        return await Task.FromResult(query.ToList());
    }
    public async Task<IList<Job>> GetSellerJobs(string userId)
    {
        var query = DbSet
                     .Where(p => p.SellerId == userId).OrderByDescending(o => o.CreatedOn);
        return await Task.FromResult(query.ToList());
    }

}
