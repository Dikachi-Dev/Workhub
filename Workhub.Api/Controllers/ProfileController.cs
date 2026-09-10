using Asp.Versioning;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workhub.Api.EndPoints;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Profiless.Commands;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Profiless.Query;
using Workhub.Contracts.Profileing;

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Vendor,User")]
[Route("api/v{version:apiVersion}/profile")]
[ApiVersion("1.0")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IFileStorageService fileStorage;

    public ProfileController(IMediator mediator, IMapper mapper, IFileStorageService fileStorage)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.fileStorage = fileStorage;
    }

    [HttpGet("myprofile")]
    public async Task<IResult> MyProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }
        var query = new MyProfileQuery(userId);
        ErrorOr<MyProfileResult> profileresult = await mediator.Send(query);

        return profileresult.Match(
            profileresult => Results.Ok(new ProfileResponse(
                profileresult.FirstName,
                profileresult.LastName,
                profileresult.PhoneNumber,
                fileStorage.GetImageUrl(profileresult.ProfileImage),
                profileresult.Country,
                profileresult.Address,
                profileresult.State,
                profileresult.Occupation,
                profileresult.Experience,
                profileresult.Rating,
                profileresult.Id)),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
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
                        ProfileImage: fileStorage.GetImageUrl(profileresult.ProfileImage),
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
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("byProximity")]
    public async Task<IResult> ByProximity(string? occupation, int pageNumber, int pageSize)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        ErrorOr<ProxyResult> response = occupation == null
            ? await mediator.Send(new GetAllByProxyQuery(userId))
            : await mediator.Send(new GetByProxQuery(userId, occupation));

        return response.Match(
            response =>
            {
                var profiles = response.Profiles.Select(profileresult =>
                    new ProfileResponse(
                        FirstName: profileresult.FirstName,
                        LastName: profileresult.LastName,
                        PhoneNumber: profileresult.PhoneNumber,
                        ProfileImage: fileStorage.GetImageUrl(profileresult.ProfileImage),
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
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [Authorize(Roles = "Both,Vendor")]
    [HttpPost("vendProfile")]
    public async Task<IResult> VendorProfileUpdate([FromForm] VendorRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new UpdateVendorProfileCommand(userId, request.Description, request.Image1, request.Image2, request.Instagram);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [Authorize(Roles = "Both,Vendor")]
    [HttpPut("EditDescription")]
    public async Task<IResult> EditDescription(string description)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new EditVendorDescriptionCommand(userId, description);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [Authorize(Roles = "Both,Vendor")]
    [HttpPut("vendorImagechange")]
    public async Task<IResult> VendorImagechange([FromForm] VendorImageChange request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new VendorImageChangeCommand(userId, request.Description, request.Image1, request.Image2, request.Instagram);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("userprofile")]
    public async Task<IResult> UserProfile(string id)
    {
        var query = new MyProfileQuery(id);
        ErrorOr<MyProfileResult> profileresult = await mediator.Send(query);
        return profileresult.Match(
            profileresult => Results.Ok(new ProfileResponse(
                profileresult.FirstName,
                profileresult.LastName,
                profileresult.PhoneNumber,
                fileStorage.GetImageUrl(profileresult.ProfileImage),
                profileresult.Country,
                profileresult.Address,
                profileresult.State,
                profileresult.Occupation,
                profileresult.Experience,
                profileresult.Rating,
                profileresult.Id)),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("vendorprofile")]
    public async Task<IResult> VendorProfileGet(string id)
    {
        var query = new GetVendorProfileQuery(id);
        ErrorOr<VendorProfileResult> result = await mediator.Send(query);
        return result.Match(
            res => Results.Ok(new
            {
                description = res.Description,
                instagram = res.Instagram,
                image1 = res.Image1,
                image2 = res.Image2,
                id = res.Id,
                firstName = res.FirstName,
                lastName = res.LastName,
                address = res.Address,
                occupation = res.Occupation,
                country = res.Country,
                state = res.State,
                rating = res.Rating,
                phoneNumber = res.PhoneNumber
            }),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("updateProfile")]
    public async Task<IResult> ProfileUpdate([FromForm] UpdateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new UpdateProfileCommand(userId, request.FirstName, request.LastName, request.PhoneNumber, request.Image);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("updateLocation")]
    public async Task<IResult> UpdateLocation(string longlat)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new UpdateLocationCommand(userId, longlat);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("delete")]
    public async Task<IResult> DeleteAccount()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new DeleteAccountCommand(userId);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("sendMessage")]
    public async Task<IResult> Message(string priority, string subject, string message)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new SendMessageCommand(userId, priority, subject, message);
        ErrorOr<bool> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("changepassword")]
    public async Task<IResult> changePass(string password, string oldpassword)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new ChangePasswordCommand(userId, password, oldpassword);
        ErrorOr<bool> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok("Password Changed"),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [AllowAnonymous]
    [HttpPost("sendResetPasswordOtp")]
    public async Task<IResult> sendResetPasswordOtp(string Email)
    {
        var command = new SendResetPasswordOtpCommand(Email);
        ErrorOr<bool> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok("OTP Sent"),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [AllowAnonymous]
    [HttpPost("confirmresetPassword")]
    public async Task<IResult> ResetPasswordOtp(string Email, string Code, string Newpassword)
    {
        var command = new ConfirmResetPasswordCommand(Email, Code, Newpassword);
        ErrorOr<bool> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok("Password Changed"),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("isSubscribed")]
    public async Task<IResult> CheckSub()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var query = new GetSubscriptionStatusQuery(userId);
        ErrorOr<Workhub.Application.Common.Models.SubResult> result = await mediator.Send(query);
        return result.Match(
            sub => Results.Ok(sub),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("Subscribe")]
    public async Task<IResult> RegSub()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new SubscribeCommand(userId);
        ErrorOr<string> result = await mediator.Send(command);
        return result.Match(
            res => Results.Ok(res),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("isVerified")]
    public async Task<IResult> CheckEmail()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var query = new GetIsVerifiedQuery(userId);
        ErrorOr<bool> result = await mediator.Send(query);
        return result.Match(
            isVerified => Results.Ok(isVerified),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
}
