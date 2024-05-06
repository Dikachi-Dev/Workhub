namespace Workhub.Contracts.Job;

public record JobResponse(
    string jobId,
    string BuyerName,
    string SellerName,
    string SellerId,
    int SellerRating,
    string Remark,
    string Status,
    string BuyerId,
    string Occupation,
    string SellerAddress,
    string BuyerAddress,
    bool isRated,
    DateTime CreatedOn);
