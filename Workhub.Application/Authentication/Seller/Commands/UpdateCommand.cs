using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Commands;

public record UpdateCommand(string userId, string FirstName,
string LastName,
string PhoneNumber,
byte[] image) : IRequest<ErrorOr<GetResult>>;

