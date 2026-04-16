using Microsoft.AspNetCore.Identity;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Services;
public class CheckVerify : ICheckVerify
{
    private readonly UserManager<GlobalUser> userManager;
    private readonly AppDataContext context;
    private readonly IEmailSender emailSender;

    public CheckVerify(UserManager<GlobalUser> userManager, AppDataContext context, IEmailSender emailSender)
    {
        this.userManager = userManager;
        this.context = context;
        this.emailSender = emailSender;
    }

    public async Task<bool> checkVerifyStats(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user.EmailConfirmed == true)
        {
            return true;
        }
        return false;
    }
    public async Task<bool> ConfirmEmail(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        var result = await userManager.VerifyChangePhoneNumberTokenAsync(user, token, user.PhoneNumber);
        if (result == true)
        {
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = false;
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    public async Task<bool> ResendOTP(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        var result = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        var body =
   $"<p>Email: {user.UserName}.</p>" +
   "<p>Confirm your email with the OTP below</p>" +
   $"<h3>{result}</h3>" +
    "<p>This code expires in 5 minutes</p>" +
   "<p>Thank you,</p>";
        var done = await emailSender.SendEmailAsync(user.Email, "New Email Verification Code", body);
        if (done == true)
        {

            return true;
        }
        return false;
    }
}