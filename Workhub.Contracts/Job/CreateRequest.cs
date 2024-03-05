namespace Workhub.Contracts.Job;

public record CreateRequest(
    string BuyerName,
    string SellerName,
    string SellerId,
    string BuyerId,
    string Occupation);