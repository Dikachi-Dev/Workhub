using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Services;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Services;

public class OtpGenerator : IotpGenerator
{
    private readonly ISeriLogger logger;
    private readonly AppDataContext context;
    public OtpGenerator(ISeriLogger logger, AppDataContext context)
    {
        this.logger = logger;
        this.context = context;
    }
    private static readonly Random random = new Random();
    public string GenerateOtp(string Email,int length = 6)
    {
        const string digits = "0123456789";
        string code = new string(Enumerable.Repeat(digits, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        var existingOtp = context.Otps.FirstOrDefault(o => o.Code == code);
        if (existingOtp != null)
        {
            return GenerateOtp(Email,length); // Recursively generate a new OTP if a duplicate is found
        }
        var otp = new Domain.Entities.Otp
        {
            Code = code,
            ExpiryTime = DateTime.UtcNow.AddMinutes(5), // Set expiry time to 5 minutes from now
            Email = Email
        };
        context.Otps.Add(otp);
        context.SaveChanges();
        logger.LogInfo($"Generated OTP for {Email}: {code}", DateTime.UtcNow);
        return code;
    }

    public bool ValidateOtp(string email, string code)
    {
        var otp = context.Otps.FirstOrDefault(o => o.Email == email && o.Code == code);
        if (otp == null)
        {
            logger.LogInfo($"OTP validation failed for {email}: {code} - OTP not found", DateTime.UtcNow);
            return false; // OTP not found
        }
        if (otp.ExpiryTime < DateTime.UtcNow)
        {
            logger.LogInfo($"OTP validation failed for {email}: {code} - OTP expired", DateTime.UtcNow);
            return false; // OTP expired
        }
        logger.LogInfo($"OTP validation succeeded for {email}: {code}", DateTime.UtcNow);
        return true; // OTP is valid
    }

    public bool DeleteOtp(string email, string code)
    {
        var otp = context.Otps.FirstOrDefault(o => o.Email == email && o.Code == code);
        if (otp == null)
        {
            logger.LogInfo($"OTP deletion failed for {email}: {code} - OTP not found", DateTime.UtcNow);
            return false; // OTP not found
        }
        context.Otps.Remove(otp);
        context.SaveChanges();
        logger.LogInfo($"OTP deleted for {email}: {code}", DateTime.UtcNow);
        return true; // OTP deleted successfully
    }
}
