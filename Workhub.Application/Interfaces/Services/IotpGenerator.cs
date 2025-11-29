using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workhub.Application.Interfaces.Services;

public interface IotpGenerator
{
    string GenerateOtp(string Email, int length = 6);
    bool ValidateOtp(string email, string code);
    bool DeleteOtp(string email, string code);
}
