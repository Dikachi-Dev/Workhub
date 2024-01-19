using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query;
public record SellerLoginQuery (
    string Email,
    string Password): IRequest<ErrorOr<SellerAuthResult>>;

