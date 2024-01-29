namespace Workhub.Contracts.Authentication;

public record SellerRegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string Country,
    string State,
    string Address,
    string Occupation,
    double Rating,
    int JobCount,
    string ProfileImage,
    string NIN,
    string Gender,
    string Experience);

