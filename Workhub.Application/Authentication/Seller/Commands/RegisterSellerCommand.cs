using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Commands;


public record RegisterSellerCommand(
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
double Rating,
    int JobCount,
    string ProfileImage,
    string NIN,
string Experience) : IRequest<ErrorOr<SellerAuthResult>>;
