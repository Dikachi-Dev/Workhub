using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workhub.Domain;
public static partial class Errors
{
    public static partial class BuyerProfile
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "BuyerProfile.DuplicateEmail",
            description: "Email already exists");

        public static Error NotFound => Error.NotFound(
            code:"BuyerProfile.NotFound",
            description: "Buyer NotFound");
    }
}


