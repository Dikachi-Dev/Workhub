namespace Workhub.Domain.Dtos;

public record ProfileResponse(string FirstName,
string LastName,
string PhoneNumber,
string ProfileImage,
string Country,
string Address,
string State,
string Occupation,
string LongLat,
string Experience,
int Rating,
string Id);
