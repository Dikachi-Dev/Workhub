using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workhub.Api.EndPoints;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Profiless.Query;
using Workhub.Contracts.Profileing;

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Seller,User")]
[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public ProfileController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }
    [HttpGet("myprofile")]
    public async Task<IResult> MyProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new MyProfileQuery(userId);
        ErrorOr<MyProfileResult> profileresult = await mediator.Send(query);
        return profileresult.Match(profileresult =>
        Results.Ok(profileresult), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("all")]
    public async Task<IResult> AllProfile()
    {
        var query = new GetAllQuery();
        ErrorOr<GetAllResult> response = await mediator.Send(query);
        return response.Match(response=> 
        Results.Ok(response), errors=>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
}
