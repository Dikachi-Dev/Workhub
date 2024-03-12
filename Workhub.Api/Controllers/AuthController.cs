using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workhub.Api.EndPoints;
using Workhub.Application.Authentication.Seller.Commands;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Authentication.Seller.Query;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.Controllers;
[AllowAnonymous]
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }
    [HttpPost("register")]
    public async Task<IResult> Register(RegisterRequest request)
    {
        var command = mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthResult> registerResult = await mediator.Send(command);
        return registerResult.Match(authResult =>
        Results.Ok(mapper.Map<LoginResponse>(authResult)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
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
    public async Task<IResult> Confirm (string email, string token)
    {
        var command = new ConfirmCommand(email,token);
        ErrorOr<ConfirmResponse> response = await mediator.Send(command);
        return response.Match(p=> Results.Ok("Verified"), errors=> Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
}
