using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Commands;

public record UpdateCommand(string FirstName,
string LastName,
string Email,
string PhoneNumber,
string Country,
string State,
string Address,
string Occupation,
string Experience) : IRequest<ErrorOr<GetResult>>;

