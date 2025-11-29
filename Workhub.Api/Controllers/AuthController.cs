using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workhub.Api.EndPoints;
using Workhub.Application.Authentication.Seller.Commands;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Authentication.Seller.Query;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Contracts.Authentication;
using Asp.Versioning;

namespace Workhub.Api.Controllers;
[AllowAnonymous]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ICheckVerify verify;
    private readonly IProfileRepository repository;

    public AuthController(IMediator mediator, IMapper mapper, ICheckVerify verify, IProfileRepository repository)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.verify = verify;
        this.repository = repository;
    }
    [HttpPost("register")]
    public async Task<IResult> Register(RegisterRequest request)
    {
        bool result = await repository.UserExista(request.Email, request.PhoneNumber);
        if (result == true)
        {
            return Results.BadRequest("User Exists");
        }
        else
        {
            var command = mapper.Map<RegisterCommand>(request);
            ErrorOr<AuthResult> registerResult = await mediator.Send(command);
            return registerResult.Match(authResult =>
            Results.Ok(new LoginResponse(authResult.token)), errors =>
            Results.Problem(EndpointBase.GetProblemDetails(errors)));
        }


    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginRequest request)
    {
        var query = mapper.Map<LoginQuery>(request);
        ErrorOr<AuthResult> loginResult = await mediator.Send(query);
        return loginResult.Match(authresult =>
        Results.Ok(new LoginResponse(authresult.token)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));

    }


    [HttpPost("confirm")]
    public async Task<IResult> Confirm(string email, string token)
    {
        var command = new ConfirmCommand(email, token);
        ErrorOr<ConfirmResponse> response = await mediator.Send(command);
        return response.Match(p => Results.Ok("Verified"), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
    [HttpPost("resend")]
    public async Task<IResult> Resend(string email)
    {
        bool result = await verify.ResendOTP(email);
        if (result == true)
        {
            return Results.Ok("New Otp Sent");
        }
        return Results.Ok("Not Sent");
    }

}
