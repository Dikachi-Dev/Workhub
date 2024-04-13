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

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Vendor,User")]
[Route("api/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IProfileRepository repository;
    private readonly IFileUpload upload;
    private readonly ISeriLogger logger;

    public ProfileController(IMediator mediator, IMapper mapper, IProfileRepository repository, ISeriLogger logger, IFileUpload upload)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.repository = repository;
        this.logger = logger;
        this.upload = upload;
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
        Results.Ok(profileresult), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("all")]
    public async Task<IResult> AllProfile(string? Filter)
    {
        var query = new GetAllQuery(Filter);
        ErrorOr<GetAllResult> response = await mediator.Send(query);
        return response.Match(response =>
        Results.Ok(response), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
    [HttpGet("byProximity")]
    public async Task<IResult> ByProximity(string occupation)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var query = new GetByProxQuery(userId,occupation);
        ErrorOr<ProxyResult> response = await mediator.Send(query);
        return response.Match(response =>
        Results.Ok(response), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [Authorize(Roles = "Both,Vendor")]
    [HttpPost("vendorProfile")]
    public async Task<IResult> VendorProfileUpdate(VendorRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var profile = await repository.GetById(userId);
        var image1 = await upload.UploadImageAsync(request.Image1);
        var image2 = await upload.UploadImageAsync(request.Image2);
        profile.VendorProfile.Description = request.Description;
        profile.VendorProfile.Image1.Description = image1.Url.ToString();
        profile.VendorProfile.Image1.publicId = image1.PublicId;
        profile.VendorProfile.Image2.Description = image2.Url.ToString();
        profile.VendorProfile.Image2.publicId = image2.PublicId;
        profile.VendorProfile.Instagram = request.Instagram;
        try{
            repository.Update(profile);
            await repository.SaveChanges();
        }catch(Exception ex){
            logger.LogExceptions(ex.Message,DateTime.UtcNow);
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
}
