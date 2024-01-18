using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query;

public class GetSellerByIdquery : IRequest<IEnumerable<SellerProfile>>
{
    public string Id { get; set; } = default!;
}
