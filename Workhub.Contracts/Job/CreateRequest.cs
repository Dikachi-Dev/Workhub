namespace Workhub.Contracts.Job;

internal record CreateRequest(
    string BuyerName,
    string SellerName,
    string SellerId,
    string BuyerId,
    string Occupation);