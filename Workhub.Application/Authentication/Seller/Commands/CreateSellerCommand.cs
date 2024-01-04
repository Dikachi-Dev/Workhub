using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Commands;


public record CreateSellerCommand(
string FirstName,
string LastName,
string Email,
string PhoneNumber,
string Password,
string Country,
string State,
string Address,
string Occupation,
string Gender,
string Experience) : IRequest<ErrorOr<SellerAuthResult>>;
