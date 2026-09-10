using Asp.Versioning;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workhub.Api.EndPoints;
using Workhub.Application.Jobber.Command;
using Workhub.Application.Jobber.Common;
using Workhub.Application.Jobber.Query;
using Workhub.Contracts.Job;
using Workhub.Domain.Entities;

namespace Workhub.Api.Controllers;

[Authorize(Roles = "Both,Vendor,User")]
[Route("api/v{version:apiVersion}/job")]
[ApiVersion("1.0")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public JobController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost("cancel")]
    public async Task<IResult> Cancel(string JobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }
        if (string.IsNullOrEmpty(JobId))
        {
            return Results.BadRequest("Job does not exist");
        }

        var command = new CanCelJobCommand(JobId, userId);
        await mediator.Send(command);
        return Results.Ok();
    }

    [HttpPost("decline")]
    public async Task<IResult> Decline(string JobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }
        if (string.IsNullOrEmpty(JobId))
        {
            return Results.BadRequest("Job does not exist");
        }

        var command = new DeclineJobCommand(JobId);
        await mediator.Send(command);
        return Results.Ok();
    }

    [HttpPost("accept")]
    public async Task<IResult> Accept(string JobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }
        if (string.IsNullOrEmpty(JobId))
        {
            return Results.BadRequest("Job does not exist");
        }

        var command = new AcceptJobCommand(JobId);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(
            res => Results.Ok(new JobResponse(
                res.Job.Id,
                res.Job.BuyerName,
                res.Job.SellerName,
                res.Job.SellerId,
                res.Job.SellerRating,
                res.Job.Remark,
                res.Job.Status,
                res.Job.BuyerId,
                res.Job.Occupation,
                res.Job.SellerAddress,
                res.Job.BuyerAddress,
                res.Job.IsRated,
                res.Job.CreatedOn)),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("manual")]
    public async Task<IResult> ManualCreate(CreateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new CreateCommand(request.BuyerName, request.SellerName, request.SellerId, userId, request.Occupation);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(
            res => Results.Ok(new JobResponse(
                res.Job.Id,
                res.Job.BuyerName,
                res.Job.SellerName,
                res.Job.SellerId,
                res.Job.SellerRating,
                res.Job.Remark,
                res.Job.Status,
                res.Job.BuyerId,
                res.Job.Occupation,
                res.Job.SellerAddress,
                res.Job.BuyerAddress,
                res.Job.IsRated,
                res.Job.CreatedOn)),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("auto")]
    public async Task<IResult> AutoCreate(AutoCreateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new AutoCreateCommand(userId, request.Occupation);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(
            res => Results.Ok(new JobResponse(
                res.Job.Id,
                res.Job.BuyerName,
                res.Job.SellerName,
                res.Job.SellerId,
                res.Job.SellerRating,
                res.Job.Remark,
                res.Job.Status,
                res.Job.BuyerId,
                res.Job.Occupation,
                res.Job.SellerAddress,
                res.Job.BuyerAddress,
                res.Job.IsRated,
                res.Job.CreatedOn)),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("getbyId")]
    public async Task<IResult> GetbyId(string Id)
    {
        var query = new GetbyIdQuery(Id);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(query);
        return jobResult.Match(
            res => Results.Ok(new JobResponse(
                res.Job.Id,
                res.Job.BuyerName,
                res.Job.SellerName,
                res.Job.SellerId,
                res.Job.SellerRating,
                res.Job.Remark,
                res.Job.Status,
                res.Job.BuyerId,
                res.Job.Occupation,
                res.Job.SellerAddress,
                res.Job.BuyerAddress,
                res.Job.IsRated,
                res.Job.CreatedOn)),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("getbyUser")]
    public async Task<IResult> GetMyJobs()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var query = new GetUserJobsQuery(userId);
        var result = await mediator.Send(query);
        return result.Match(
            jobs => Results.Ok(jobs),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("UserRate")]
    public async Task<IResult> UserRate(string jobid, int rating, string remark)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Results.BadRequest("User Not Found");
        }

        var command = new RateJobCommand(jobid, rating, remark);
        ErrorOr<Success> result = await mediator.Send(command);
        return result.Match(
            _ => Results.Ok(),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("getUserReviews")]
    public async Task<IResult> UserReviews(string userId)
    {
        var query = new GetUserReviewsQuery(userId);
        ErrorOr<List<ReviewDto>> result = await mediator.Send(query);
        return result.Match(
            reviews => Results.Ok(reviews.Select(r => new
            {
                Name = r.Name,
                Rating = r.Rating,
                Remark = r.Remark
            })),
            errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }
}
