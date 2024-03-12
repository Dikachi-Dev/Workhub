using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workhub.Application.Interfaces.Persistance
{
    public interface ICheckVerify
    {
        Task<bool> checkVerifyStats(string email);
        Task<bool> ConfirmEmail(string token,string email );
    }
}