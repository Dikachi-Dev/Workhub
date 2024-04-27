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

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Vendor,User")]
[Route("api/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IProfileRepository repository;
    private readonly ICloseProx closeProx;
    private readonly ISeriLogger logger;

    public ProfileController(IMediator mediator, IMapper mapper, IProfileRepository repository, ISeriLogger logger, ICloseProx closeProx)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.repository = repository;
        this.logger = logger;
        this.closeProx = closeProx;

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
        Results.Ok(new MyProfileResponse(profileresult.FirstName, profileresult.LastName, profileresult.Email, profileresult.PhoneNumber, FileHelper.GetDoc(profileresult.ProfileImage), profileresult.Country, profileresult.Address, profileresult.State, profileresult.Occupation, profileresult.Gender, profileresult.Experience, profileresult.Rating, profileresult.JobCount, profileresult.Token, profileresult.Id, profileresult.LongLat, profileresult.UserType)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("all")]
    public async Task<IResult> AllProfile(string? Filter)
    {
        var query = new GetAllQuery(Filter);
        ErrorOr<GetAllResult> response = await mediator.Send(query);
        return response.Match(
            response =>
            {
                var profiles = response.Profiles.Select(profileresult =>
                    new MyProfileResponse(
                        FirstName: profileresult.FirstName,
                        LastName: profileresult.LastName,
                        Email: profileresult.Email,
                        PhoneNumber: profileresult.PhoneNumber,
                        ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
                        Country: profileresult.Country,
                        Address: profileresult.Address,
                        State: profileresult.State,
                        Occupation: profileresult.Occupation,
                        Gender: profileresult.Gender,
                        Experience: profileresult.Experience,
                        Rating: profileresult.Rating,
                        JobCount: profileresult.JobCount,
                        Token: profileresult.Token,
                        Id: profileresult.Id,
                        LongLat: profileresult.LongLat,
                        UserType: profileresult.UserType
                    )
                );
                return Results.Ok(new GetAllResponse(profiles));
            },
            errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
    [HttpGet("byProximity")]
    public async Task<IResult> ByProximity(string? occupation)
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
                        new MyProfileResponse(
                            FirstName: profileresult.FirstName,
                            LastName: profileresult.LastName,
                            Email: profileresult.Email,
                            PhoneNumber: profileresult.PhoneNumber,
                            ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
                            Country: profileresult.Country,
                            Address: profileresult.Address,
                            State: profileresult.State,
                            Occupation: profileresult.Occupation,
                            Gender: profileresult.Gender,
                            Experience: profileresult.Experience,
                            Rating: profileresult.Rating,
                            JobCount: profileresult.JobCount,
                            Token: profileresult.Token,
                            Id: profileresult.Id,
                            LongLat: profileresult.LongLat,
                            UserType: profileresult.UserType
                        )
                    ).ToList(); // Convert to a list
                    return Results.Ok(new GetAllResponse(profiles));
                },
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
                        new MyProfileResponse(
                            FirstName: profileresult.FirstName,
                            LastName: profileresult.LastName,
                            Email: profileresult.Email,
                            PhoneNumber: profileresult.PhoneNumber,
                            ProfileImage: FileHelper.GetDoc(profileresult.ProfileImage),
                            Country: profileresult.Country,
                            Address: profileresult.Address,
                            State: profileresult.State,
                            Occupation: profileresult.Occupation,
                            Gender: profileresult.Gender,
                            Experience: profileresult.Experience,
                            Rating: profileresult.Rating,
                            JobCount: profileresult.JobCount,
                            Token: profileresult.Token,
                            Id: profileresult.Id,
                            LongLat: profileresult.LongLat,
                            UserType: profileresult.UserType
                        )
                    ).ToList(); // Convert to a list
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

        string image1 = FileHelper.CreateDocFile(request.Image1, userId + "Image1" + request.Image1Ext);
        string image2 = FileHelper.CreateDocFile(request.Image2, userId + "Image2" + request.Image2Ext);

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

    [HttpGet("userprofile")]
    public async Task<IResult> UserProfile(string id)
    {
        var query = new MyProfileQuery(id);
        ErrorOr<MyProfileResult> profileresult = await mediator.Send(query);
        return profileresult.Match(profileresult =>
        Results.Ok(profileresult), errors =>
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
        return Results.Ok(new { profile.VendorProfile.Description, profile.VendorProfile.Instagram, image1, image2 });
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

        string image = FileHelper.CreateDocFile(request.image, userId + "profileImage" + request.ext);

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

}
