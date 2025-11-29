using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workhub.Api.EndPoints;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Profiless.Query;
using Workhub.Contracts.Profileing;
using Workhub.Infrastructure.Services;
using static Workhub.Infrastructure.Services.CloseProx;
using Asp.Versioning;

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Vendor,User")]
[Route("api/v{version:apiVersion}/profile")]
[ApiVersion("1.0")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IProfileRepository repository;
    private readonly ICloseProx closeProx;
    private readonly ISeriLogger logger;
    private readonly IEmailSender emailSender;
    public ProfileController(IMediator mediator, IMapper mapper, IProfileRepository repository, ISeriLogger logger, ICloseProx closeProx, IEmailSender emailSender)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.repository = repository;
        this.logger = logger;
        this.closeProx = closeProx;
        this.emailSender = emailSender;

        //this.upload = upload;
    }
    [HttpGet("myprofile")]
    public async Task<IResult> MyProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var query = new MyProfileQuery(userId);
        ErrorOr<MyProfileResult> profileresult = await mediator.Send(query);

        return profileresult.Match(profileresult =>
        Results.Ok(new ProfileResponse(profileresult.FirstName, profileresult.LastName, profileresult.PhoneNumber, FileHelper.GetDoc(profileresult.ProfileImage), profileresult.Country, profileresult.Address, profileresult.State, profileresult.Occupation, profileresult.Experience, profileresult.Rating, profileresult.Id)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("all")]
    public async Task<IResult> AllProfile(string? Filter, int pageNumber, int pageSize)
    {
        var query = new GetAllQuery(Filter, pageNumber, pageSize);
        ErrorOr<GetAllResult> response = await mediator.Send(query);
        return response.Match(
            response =>
            {
                var profiles = response.Profiles.Select(profileresult =>
                    new ProfileResponse(
                        FirstName: profileresult.FirstName,
                        LastName: profileresult.LastName,
                        PhoneNumber: profileresult.PhoneNumber,
                        ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
                        Country: profileresult.Country,
                        Address: profileresult.Address,
                        State: profileresult.State,
                        Occupation: profileresult.Occupation,
                        Experience: profileresult.Experience,
                        Rating: profileresult.Rating,
                        Id: profileresult.Id
                    )
                );
                return Results.Ok(new GetAllResponse(profiles));
            },
            errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
    [HttpGet("byProximity")]
    public async Task<IResult> ByProximity(string? occupation, int pageNumber, int pageSize)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        if (occupation == null)
        {
            var query = new GetAllByProxyQuery(userId);
            ErrorOr<ProxyResult> response = await mediator.Send(query);
            return response.Match(
                response =>
                {
                    var profiles = response.Profiles.Select(profileresult =>
                        new ProfileResponse(
                            FirstName: profileresult.FirstName,
                            LastName: profileresult.LastName,
                            PhoneNumber: profileresult.PhoneNumber,
                            ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
                            Country: profileresult.Country,
                            Address: profileresult.Address,
                            State: profileresult.State,
                            Occupation: profileresult.Occupation,
                            Experience: profileresult.Experience,
                            Rating: profileresult.Rating,
                            Id: profileresult.Id
                        )
                    ).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    return Results.Ok(new GetAllResponse(profiles));
                },
            //    {
            //    var profiles = response.Profiles.Select(profileresult =>
            //        new MyProfileResponse(
            //            FirstName: profileresult.FirstName,
            //            LastName: profileresult.LastName,
            //            Email: profileresult.Email,
            //            PhoneNumber: profileresult.PhoneNumber,
            //            ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
            //            Country: profileresult.Country,
            //            Address: profileresult.Address,
            //            State: profileresult.State,
            //            Occupation: profileresult.Occupation,
            //            Gender: profileresult.Gender,
            //            Experience: profileresult.Experience,
            //            Rating: profileresult.Rating,
            //            JobCount: profileresult.JobCount,
            //            Token: profileresult.Token,
            //            Id: profileresult.Id,
            //            LongLat: profileresult.LongLat,
            //            UserType: profileresult.UserType
            //        )
            //    ).ToList(); // Convert to a list
            //    return Results.Ok(new GetAllResponse(profiles));
            //},
                errors =>

            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }
        else
        {
            var query = new GetByProxQuery(userId, occupation);
            ErrorOr<ProxyResult> response = await mediator.Send(query);
            return response.Match(
                response =>
                {
                    var profiles = response.Profiles.Select(profileresult =>
                        new ProfileResponse(
                            FirstName: profileresult.FirstName,
                            LastName: profileresult.LastName,
                            PhoneNumber: profileresult.PhoneNumber,
                            ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
                            Country: profileresult.Country,
                            Address: profileresult.Address,
                            State: profileresult.State,
                            Occupation: profileresult.Occupation,
                            Experience: profileresult.Experience,
                            Rating: profileresult.Rating,
                            Id: profileresult.Id
                        )
                    ).Skip((pageNumber - 1) * pageSize).Take(pageSize);// Convert to a list
                    return Results.Ok(new GetAllResponse(profiles));
                },
                errors =>

            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }
    }

    [Authorize(Roles = "Both,Vendor")]
    [HttpPost("vendProfile")]
    public async Task<IResult> VendorProfileUpdate(VendorRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = repository.GetVendor(userId);

        byte[] imag1 = FileHelper.GetResizedImage(request.Image1, 200);
        byte[] imag2 = FileHelper.GetResizedImage(request.Image2, 200);

        string image1 = FileHelper.CreateDocFile(imag1, userId + "Image1" + "png");
        string image2 = FileHelper.CreateDocFile(imag2, userId + "Image2" + "png");
        profile.VendorProfile.Image1ext = "png";
        profile.VendorProfile.Image2ext = "png";
        profile.VendorProfile.Description = request.Description;
        profile.VendorProfile.Image1 = image1;
        profile.VendorProfile.Image2 = image2;
        profile.VendorProfile.Instagram = request.Instagram;
        try
        {
            repository.Update(profile);
            await repository.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogExceptions(ex.Message, DateTime.UtcNow);
        }
        return Results.Ok();
    }
    [Authorize(Roles = "Both,Vendor")]
    [HttpPut("EditDescription")]
    public async Task<IResult> EditDescription(string description)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = repository.GetVendor(userId);

        profile.VendorProfile.Description = description;
        try
        {
            repository.Update(profile);
            await repository.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogExceptions(ex.Message, DateTime.UtcNow);
        }
        return Results.Ok();
    }


    [Authorize(Roles = "Both,Vendor")]
    [HttpPut("vendorImagechange")]
    public async Task<IResult> VendorImagechange(VendorImageChange request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = repository.GetVendor(userId);

        byte[] imag1 = FileHelper.GetResizedImage(request.Image1, 200);
        byte[] imag2 = FileHelper.GetResizedImage(request.Image2, 200);

        string image1 = FileHelper.CreateDocFile(imag1, userId + "Image1" + "png");
        string image2 = FileHelper.CreateDocFile(imag2, userId + "Image2" + "png");
        profile.VendorProfile.Image1ext = "png";
        profile.VendorProfile.Image2ext = "png";
        profile.VendorProfile.Image1 = image1;
        profile.VendorProfile.Image2 = image2;
        profile.VendorProfile.Description = request.Description;
        profile.VendorProfile.Instagram = request.Instagram;
        try
        {
            repository.Update(profile);
            await repository.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogExceptions(ex.Message, DateTime.UtcNow);
        }
        return Results.Ok();
    }

    [HttpGet("userprofile")]
    public async Task<IResult> UserProfile(string id)
    {
        var query = new MyProfileQuery(id);
        ErrorOr<MyProfileResult> profileresult = await mediator.Send(query);
        return profileresult.Match(profileresult =>
                Results.Ok(new ProfileResponse(profileresult.FirstName, profileresult.LastName, profileresult.PhoneNumber, FileHelper.GetDoc(profileresult.ProfileImage), profileresult.Country, profileresult.Address, profileresult.State, profileresult.Occupation, profileresult.Experience, profileresult.Rating, profileresult.Id)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
    [HttpGet("vendorprofile")]
    public async Task<IResult> VendorProfileGet(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = repository.GetVendor(id);
        byte[] image1 = FileHelper.GetDoc(profile.VendorProfile.Image1);
        byte[] image2 = FileHelper.GetDoc(profile.VendorProfile.Image2);
        return Results.Ok(new { profile.VendorProfile.Description, profile.VendorProfile.Instagram, image1, image2, profile.VendorProfile.Image1ext, profile.VendorProfile.Image2ext, profile.Id, profile.FirstName, profile.LastName, profile.Address, profile.Occupation, profile.Country, profile.State, profile.Rating, profile.PhoneNumber, });
    }


    [HttpPost("updateProfile")]
    public async Task<IResult> ProfileUpdate(UpdateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = await repository.GetById(userId);
        byte[] imag = FileHelper.GetResizedImage(request.image, 160);
        string image = FileHelper.CreateDocFile(imag, userId + "profileImage" + "png");

        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.PhoneNumber = request.PhoneNumber;
        profile.ProfileImage = image;
        try
        {
            repository.Update(profile);
            await repository.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogExceptions(ex.Message, DateTime.UtcNow);
        }
        return Results.Ok();
    }

    [HttpPost("updateLocation")]
    public async Task<IResult> UpdateLocation(string longlat)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = await repository.GetById(userId);
        FullAddress response = await closeProx.GetFullAddress(longlat);
        if (response != null)
        {

            profile.LongLat = longlat;
            profile.Country = response.Country;
            profile.State = response.State;
            profile.Address = response.Address;
            try
            {
                repository.Update(profile);
                await repository.SaveChanges();
                logger.LogInfo($"{profile.Email} upadted location to {longlat}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                logger.LogExceptions(ex.Message, DateTime.UtcNow);
            }
            return Results.Ok();
        }
        else
        {
            return Results.BadRequest("Location Failed to Update");
        }
    }
    [HttpPost("delete")]
    public async Task<IResult> DeleteAccount()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = await repository.GetById(userId);
        profile.isDeleted = true;
        await repository.DeleteUser(userId);
        //await repository.SaveChanges();
        return Results.Ok();
    }

    [HttpPost("sendMessage")]
    public async Task<IResult> Message(string priority, string subject, string message)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        string to = configuration.GetSection("Smtp:Email").Value;
        var profile = await repository.GetById(userId);
        string body = $"<p>{message}</p>";
        await emailSender.SendEmailAsync(to, $"Priority:{priority} From: {profile.Email}, Subject: {subject}", body);
        return Results.Ok();
    }
    [HttpPost("changepassword")]
    public async Task<IResult> changePass(string password, string oldpassword)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        bool result = await repository.ChangePass(password, userId, oldpassword);
        if (result)
        {
            return Results.Ok("Password Changed");
        }
        return Results.BadRequest("Failed to change password");
    }
    [AllowAnonymous]
    [HttpPost("sendResetPasswordOtp")]
    public async Task<IResult> sendResetPasswordOtp(string Email)
    {
        bool result = await repository.ResetPassCode(Email);
        if (result)
        {
            return Results.Ok("Password Changed");
        }
        return Results.BadRequest("Error while trying to retrive the User");
    }

    [AllowAnonymous]
    [HttpPost("confirmresetPassword")]
    public async Task<IResult> ResetPasswordOtp(string Email, string Code, string Newpassword)
    {
        bool result = await repository.ResetPassword(Email, Code, Newpassword);
        if (result)
        {
            return Results.Ok("Password Changed");
        }
        return Results.BadRequest("Error while trying to retrive the User");
    }

    [HttpGet("isSubscribed")]
    public async Task<IResult> CheckSub()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var result = await repository.IsSubscribed(userId);
        return Results.Ok(result);
    }
    [HttpPost("Subscribe")]
    public async Task<IResult> RegSub()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var result = await repository.Subscribed(userId);
        return Results.Ok(result);
    }

    [HttpGet("isVerified")]
    public async Task<IResult> CheckEmail()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var result = await repository.isVerified(userId);
        return Results.Ok(result);
    }
}

