using ErrorOr;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;

namespace Workhub.Application.Authentication.Buyer.Commands;

public record RegisterBuyerCommand(string FirstName,
 string LastName,
 string Email,
 string PhoneNumber,
 string Address,
 string State,
 string Country,
 string Password) : IRequest<ErrorOr<BuyerAuthResult>>;

