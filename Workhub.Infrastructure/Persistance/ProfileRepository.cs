using Microsoft.AspNetCore.Identity;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
{
    private readonly UserManager<GlobalUser> userManager;
    private readonly IJWTGenerator jWTGenerator;
    private readonly RoleManager<IdentityRole> roleManager;
    public ProfileRepository(AppDataContext context, UserManager<GlobalUser> userManager, IJWTGenerator jWTGenerator, RoleManager<IdentityRole> roleManager) : base(context)
    {
        this.userManager = userManager; // Assigning the injected userManager
        this.jWTGenerator = jWTGenerator; // Assigning the injected jWTGenerator
        this.roleManager = roleManager;
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

    public async Task<GlobalUser> Login(string username, string password)
    {
        var user = await userManager.FindByEmailAsync(username);
        if (user != null && await userManager.CheckPasswordAsync(user, password))
        {
            var userRole = await userManager.GetRolesAsync(user);
            if (userRole.Contains("User"))
            {
                return user;
            }
            return null;

        }
        return null;
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

        var roleName = "User";
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            // Role doesn't exist, create it
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }

        // Use roleName variable here instead of hardcoding "User"
        await userManager.AddToRoleAsync(user, roleName);
        bool isCompleted = true; // If the execution reaches this line, it means the operation is completed
        await Add(profile);
        await SaveChanges();
        return await userManager.FindByEmailAsync(profile.Email);

    }
}