using Microsoft.AspNetCore.Http;

namespace Workhub.Contracts.Profileing;

public record VendorRequest(string Description, IFormFile? Image1, IFormFile? Image2, string Instagram);
public record VendorImageChange(string Description, IFormFile? Image1, IFormFile? Image2, string Instagram);
