namespace Workhub.Contracts.Authentication;
public record BuyerResponse(
    string userId,
    string FirstName,
    string LastName,
    string PhoneNumber);