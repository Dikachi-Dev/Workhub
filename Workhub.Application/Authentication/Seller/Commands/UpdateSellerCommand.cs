using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Authentication.Seller.Common;

namespace Workhub.Application.Authentication.Seller.Commands;

public record UpdateSellerCommand(string FirstName,
string LastName,
string Email,
string PhoneNumber,
string Country,
string State,
string Address,
string Occupation,
string Experience) : IRequest<ErrorOr<SellerAuthResult>>;

