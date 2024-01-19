using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Domain.Entities;

namespace Workhub.Application.Authentication.Buyer.Common;

public record BuyerAuthResult(
    BuyerProfile buyerProfile, 
    string token);

