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
    public ProfileRepository(AppDataContext context, UserManager<GlobalUser> userManager, IJWTGenerator jWTGenerator, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IConfiguration config) : base(context)
    {
        this.userManager = userManager; // Assigning the injected userManager
        this.jWTGenerator = jWTGenerator; // Assigning the injected jWTGenerator
        this.roleManager = roleManager;
        this.emailSender = emailSender;
        _config = config;
    }

    //public ProfileRepository(AppDataContext context) : base(context)
    //{
    //    this.userManager = userManager;
    //    this.jWTGenerator = jWTGenerator;
    //}

    public IQueryable<Profile?> GetByFilter(string filter)
    {
        return GetAll()
           .Where(profile => profile != null && profile.FirstName
           .Contains(filter) || profile.Email
           .Contains(filter) || profile.LastName
           .Contains(filter) || profile.State
           .Contains(filter) || profile.Country
           .Contains(filter));
    }

    public IQueryable<Profile?> GetQueryableSellerProfiles()
    {
        throw new NotImplementedException();
    }

    public Profile? GetProfileByEmail(string email)
    {
        return GetAll().FirstOrDefault(x => x.Email == email);
    }


    public Profile? GetSellerProfileByIdAllWithCollections(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Claim>> Login(string username, string password)
    {
        var user = await userManager.FindByEmailAsync(username);
        if (user != null && await userManager.CheckPasswordAsync(user, password))
        {
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains("User") || roles.Contains("Admin") || roles.Contains("Seller") || roles.Contains("Both"))
            {
                // Create claims for the user including roles
                var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim(ClaimTypes.Email, user.Email)
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
            UserName = profile.Email
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
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var url = $"{_config["ValidUrl"]}/{_config["ConfirmMail"]}?email={user.Email}&token={token}";

        var body = $"<p>Hello: {user.Email} </p>" +
           $"<p>Username: {user.UserName}.</p>" +
           "<p>In Order to confirm your email, please click on the following link.</p>" +
           $"<p><a href=\"{url}\">Click here</a></p>" +
           "<p>Thank you,</p>" +
           $"<br>{_config["Email:ApplicationName"]}";
        await emailSender.SendEmailAsyncMimeKit(user.Email, "Email Verification", body);
        await Add(profile);
        await SaveChanges();
        return await userManager.FindByEmailAsync(profile.Email);

    }

    public IQueryable<Profile?> GetByProximity()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Profile>> GetByOccupation(string occupation)
    {
        var profiles = await DbSet.Where(p => p.Occupation == occupation).ToListAsync();
        return profiles;
    }
}