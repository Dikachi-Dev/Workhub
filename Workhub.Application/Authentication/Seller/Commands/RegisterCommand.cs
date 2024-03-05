using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Commands;


public record RegisterCommand(
string FirstName,
string LastName,
string Email,
string Password,
string PhoneNumber,
string Country,
string State,
string Address,
string Occupation,
string Gender,
string LongLat,
string UserType,
string ProfileImage,
string Nin,
string Experience) : IRequest<ErrorOr<AuthResult>>;
