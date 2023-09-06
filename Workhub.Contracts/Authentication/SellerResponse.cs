using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workhub.Contracts.Authentication;
public record SellerResponse(
    string userId,
    string FirstName,
    string LastName,
    string PhoneNumber);
