using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IJobRepository : IGenericRepository<Job>
{
    Task<Job> CreateJob(string userId, string occupation);
    Task<IList<Job>> GetUserJobs(string userId);
    Task<Job> Accept(string jobId);
    Task<Job> Decline(string jobId);
    void Cancel(string jobId);
}

public record responseJob(
    string jobId,
    string BuyerName,
    string SellerName,
    string SellerId,
    double SellerRating,
    double BuyerRating,
    string Status,
    DateTime CreatedOn,
    string BuyerId,
    string Occupation);
