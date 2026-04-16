namespace Workhub.Contracts.Profileing;

public record GetAllResponse(IEnumerable<ProfileResponse> Profiles);

public record ProfileResponse(string FirstName,
 string LastName,
 string PhoneNumber,
 byte[] ProfileImage,
 string Country,
 string Address,
 string State,
 string Occupation,
 string Experience,
 int Rating,
 string Id);
