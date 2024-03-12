using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Services;
public class CheckVerify : ICheckVerify
{
    private readonly UserManager<GlobalUser> userManager;

    public CheckVerify(UserManager<GlobalUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<bool> checkVerifyStats(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if(user.EmailConfirmed == true)
        {
            return true;
        }
        return false;
    }
    public async Task<bool> ConfirmEmail(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }
}