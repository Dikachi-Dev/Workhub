namespace Workhub.Contracts.Authentication;
public record Response(
    string userId,
    string FirstName,
    string LastName,
    string PhoneNumber);
