namespace Workhub.Contracts.Authentication;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string Country,
    string State,
    string Address,
    string Occupation,
    string UserType,
    string ProfileImage,
    string Nin,
    string Lga,
    string LongLat,
    string Gender,
    string Experience);

