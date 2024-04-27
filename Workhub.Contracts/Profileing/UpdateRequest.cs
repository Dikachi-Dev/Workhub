namespace Workhub.Contracts.Profileing;

public record UpdateRequest(string FirstName,
string LastName,
string PhoneNumber,
byte[] image, string ext);