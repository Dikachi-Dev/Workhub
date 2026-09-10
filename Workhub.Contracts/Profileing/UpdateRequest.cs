using Microsoft.AspNetCore.Http;

namespace Workhub.Contracts.Profileing;

public record UpdateRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    IFormFile? Image);