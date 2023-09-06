namespace Workhub.Contracts.Authentication;

public record BuyerRegisterRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        string Password,
        string Country,
        string State,
        string Address);


