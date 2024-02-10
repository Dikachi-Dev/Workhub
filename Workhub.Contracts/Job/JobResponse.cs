namespace Workhub.Contracts.Job;

internal record JobResponse(
    string jobId,
    string BuyerName,
    string SellerName,
    string Rating,
    string Status,
    string BuyerId,
    string Occupation);
