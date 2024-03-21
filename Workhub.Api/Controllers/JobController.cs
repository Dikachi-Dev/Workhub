using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workhub.Api.EndPoints;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Jobber.Command;
using Workhub.Application.Jobber.Common;
using Workhub.Application.Jobber.Query;
using Workhub.Contracts.Job;

namespace Workhub.Api.Controllers;
[Authorize(Roles = "Both,Seller,User")]
[Route("api/job")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IJobRepository repository;

    public JobController(IMediator mediator, IMapper mapper, IJobRepository repository)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpPost("manual")]
    public async Task<IResult> ManualCreate(CreateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new CreateCommand(request.BuyerName, request.SellerName, request.SellerId, userId, request.Occupation);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerRating, jobResult.Job.BuyerRating, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("auto")]
    public async Task<IResult> AutoCreate(AutoCreateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new AutoCreateCommand(userId, request.Occupation);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerRating, jobResult.Job.BuyerRating, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("getbyId")]
    public async Task<IResult> GetbyId(string Id)
    {
        var query = new GetbyIdQuery(Id);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(query);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerRating, jobResult.Job.BuyerRating, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("getbyUser")]
    public async Task<IResult> GetMyJobs()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
         var result = await repository.GetUserJobs(userId);
         return Results.Ok(result);
    }
}
