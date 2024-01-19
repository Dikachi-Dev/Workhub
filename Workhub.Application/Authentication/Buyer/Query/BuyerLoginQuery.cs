using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Authentication.Buyer.Common;

namespace Workhub.Application.Authentication.Buyer.Query;

public record BuyerLoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<BuyerAuthResult>>;

