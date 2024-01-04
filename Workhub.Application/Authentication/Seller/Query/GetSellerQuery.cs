using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Seller.Query;
public class GetSellerQuery : IRequest<IEnumerable<SellerProfile>>
{

}
