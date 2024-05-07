using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;


namespace Workhub.Infrastructure.Persistance;

public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
{
    private readonly UserManager<GlobalUser> userManager;
    private readonly IJWTGenerator jWTGenerator;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IEmailSender emailSender;
    private readonly IConfiguration _config;
    private readonly AppDataContext appDataContext;
    public ProfileRepository(AppDataContext context, UserManager<GlobalUser> userManager, IJWTGenerator jWTGenerator, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IConfiguration config, AppDataContext appDataContext) : base(context)
    {
        this.userManager = userManager; // Assigning the injected userManager
        this.jWTGenerator = jWTGenerator; // Assigning the injected jWTGenerator
        this.roleManager = roleManager;
        this.emailSender = emailSender;
        _config = config;
        this.appDataContext = appDataContext;
    }
    public IQueryable<Profile?> GetByFilter(string filter, int pageNumber, int pageSize)
    {
        return appDataContext.Profiles
           .Where(profile => profile != null && profile.UserType != "User" && profile.isDeleted != true && profile.VendorProfile.Image1 != "" && profile.FirstName
           .Contains(filter) || profile.Email
           .Contains(filter) || profile.LastName
           .Contains(filter) || profile.State
           .Contains(filter) || profile.Country
           .Contains(filter)).OrderByDescending(o => o.Rating).Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public IQueryable<Profile?> GetQueryableSellerProfiles()
    {
        throw new NotImplementedException();
    }

    public Profile? GetProfileByEmail(string email)
    {
        return appDataContext.Profiles.FirstOrDefault(x => x.Email == email);
    }

    public IQueryable<Profile> GetAllVendors(int pageNumber, int pageSize)
    {
        return DbSet.Where(p => p.UserType != "User" && p.VendorProfile.Image1 != "" && p.isDeleted != true).OrderByDescending(o => o.Rating).Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public Profile? GetSellerProfileByIdAllWithCollections(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Claim>> Login(string username, string password, string token)
    {
        var user = await userManager.FindByEmailAsync(username);
        if (user != null && await userManager.CheckPasswordAsync(user, password))
        {
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains("User") || roles.Contains("Admin") || roles.Contains("Vendor") || roles.Contains("Both"))
            {
                var profile = GetProfileByEmail(username);
                profile.Token = token;
                Update(profile);
                await SaveChanges();

                // Create claims for the user including roles
                var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.GivenName,profile.FirstName)
};

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                // Return the list of claims
                return claims;

            }
            return [];
        }
        return [];
    }

    public async Task<GlobalUser> Register(Profile profile)
    {
        var user = new GlobalUser()
        {
            Id = profile.Id,
            Email = profile.Email,
            Password = profile.Password,
            UserName = profile.Email,
            PhoneNumber = profile.PhoneNumber,
        };
        var result = await userManager.CreateAsync(user, profile.Password);

        var roleName = profile.UserType;
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            // Role doesn't exist, create it
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }

        // Use roleName variable here instead of hardcoding "User"
        await userManager.AddToRoleAsync(user, roleName);
        var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, profile.PhoneNumber);
        //var url = $"{_config["ValidUrl"]}/{_config["ConfirmMail"]}?email={user.Email}&token={token}";

        var body = $"<h4>Hello: {profile.FirstName} </h4>" +

           "<p>Your verificationcode is </p>" +
           $"<h2>{token}</h2>" +
            "<p>This code expires in 5 minutes</p>" +
           "<p>Thank you,</p>" +
           $"<br>{_config["Email:ApplicationName"]}";
        emailSender.SendEmailAsyncMimeKit(user.Email, "Email Verification", body);
        await Add(profile);
        await SaveChanges();
        return await userManager.FindByEmailAsync(profile.Email);

    }

    public IQueryable<Profile?> GetByProximity()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Profile>> GetByOccupation(string occupation, string country)
    {
        var profileList = await DbSet.Where(p => p.UserType != "User" && p.isDeleted != true && p.Country == country).ToListAsync();
        var profiles = profileList.Where(p => p.Occupation.Split(',').Contains(occupation));
        return profiles;
    }

    public async Task<IEnumerable<Profile>> GetAllVendros(string country)
    {
        var profiles = await DbSet.Where(p => p.UserType != "User" && p.isDeleted != true && p.Country == country).ToListAsync();
        return profiles;
    }

    public Profile GetVendor(string id)
    {
        var profile = DbSet.Where(p => p.UserType != "User" && p.Id == id).Include(p => p.VendorProfile).FirstOrDefault();
        return profile;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var user = userManager.Users.FirstOrDefault(p => p.Id == id);
        await userManager.DeleteAsync(user);
        return true;
    }
    public async Task<bool> ChangePass(string password, string id, string oldpass)
    {
        var profile = await GetById(id);
        var user = userManager.Users.FirstOrDefault(p => p.Id == id);
        await userManager.ChangePasswordAsync(user, oldpass, password);
        profile.Password = password;
        Update(profile);
        await SaveChanges();
        return true;
    }

    public async Task<bool> ResetPassCode(string email)
    {
        var user = userManager.Users.FirstOrDefault(p => p.Email == email);
        if (user == null)
        {
            return false;
        }
        else
        {
            string code = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            var body = $"<h4>Hello: {user.Email} </h4>" +

              "<p>Your OTP code is </p>" +
              $"<h2>{code}</h2>" +
              "<p>This code expires in 5 minutes</p>" +
              "<p>Thank you,</p>" +
              $"<br>{_config["Email:ApplicationName"]}";
            emailSender.SendEmailAsyncMimeKit(user.Email, "Reset Password Code", body);
            return true;
        }

    }

    public async Task<bool> ResetPassword(string email, string token, string newpassword)
    {

        var user = userManager.Users.FirstOrDefault(p => p.Email == email);
        if (user == null)
        {
            return false;
        }
        else
        {
            var profile = await GetById(user.Id);
            var result = await userManager.VerifyChangePhoneNumberTokenAsync(user, token, user.PhoneNumber);
            if (result)
            {
                var newtoken = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, newtoken, newpassword);
                profile.Password = newpassword;
                Update(profile);
                await SaveChanges();
                return true;
            }
            return false;
        }

    }
    public async Task<SubResult> Subscribed(string userId)
    {
        var profile = await GetById(userId);
        DateTime his = profile.Subscribe.ExpireOn.Date;
        profile.Subscribe.IsSubscribed = true;
        profile.Subscribe.SubscribeOn = DateTime.UtcNow.Date;
        profile.Subscribe.ExpireOn = his < DateTime.UtcNow.Date ? DateTime.Now.Date.AddYears(1) : his.AddYears(1);
        var newSub = new SubHistory
        {
            Country = profile.Country,
            SubscriberId = profile.Id,
        };
        appDataContext.SubHistorys.Add(newSub);
        Update(profile);
        await SaveChanges();
        var newresult = new SubResult(profile.Subscribe.ExpireOn, true, profile.Subscribe.IsSubscribed);
        return newresult;
    }

    public async Task<SubResult> IsSubscribed(string userId)
    {
        var profile = await GetById(userId);
        var comsub = appDataContext.Subscriptions.FirstOrDefault();
        var newresult = new SubResult(profile.Subscribe.ExpireOn, comsub.IsEnabled, profile.Subscribe.IsSubscribed);
        return newresult;


    }
    public async Task<bool> UserExista(string email, string phonenumber)
    {
        var user = await userManager.FindByEmailAsync(email);
        var newone = userManager.Users.Where(p => p.PhoneNumber == phonenumber);
        if (user == null && newone == null)
        {
            return true;
        }
        return false;
    }
    public async Task<bool> isVerified(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user.EmailConfirmed)
        {
            return true;
        }
        return false;
    }

}
