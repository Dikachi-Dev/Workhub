using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IJobRepository : IGenericRepository<Job>
{

    Task<IList<Job>> GetUserJobs(string userId);
    Task<IList<Job>> GetSellerJobs(string userId);
    Task<Job> Accept(string jobId);
    Task<Job> Decline(string jobId);
    void Cancel(string jobId);
    Task<Job> Remark(string jobId, int rating, string remark);
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
