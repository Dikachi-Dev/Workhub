using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workhub.Application.Interfaces.JWT;

public interface IJWTGenerator
{
    string GenerateJWTToken(string email, string userid);
}
