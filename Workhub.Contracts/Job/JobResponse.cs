namespace Workhub.Contracts.Job;

public record JobResponse(
    string jobId,
    string BuyerName,
    string SellerName,
    string SellerId,
    double SellerRating,
    double BuyerRating,
    string Status,
    string BuyerId,
    string Occupation);
