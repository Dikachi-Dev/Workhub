using Microsoft.AspNetCore.Http;

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
    IFormFile ProfileImage,
    string Nin,
    string LongLat,
    string Token,
    string Gender,
    string Experience);

