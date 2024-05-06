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
[Authorize(Roles = "Both,Vendor,User")]
[Route("api/job")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IJobRepository repository;
    private readonly IProfileRepository profileRepository;

    public JobController(IMediator mediator, IMapper mapper, IJobRepository repository, IProfileRepository profileRepository)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.repository = repository;
        this.profileRepository = profileRepository;
    }

    [HttpPost("cancel")]
    public IResult Cancel(string JobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        if (JobId == null || JobId is "")
        {
            return Results.BadRequest("Job does not exist");
        }
        var command = new CanCelJobCommand(JobId, userId);
        if (mediator.Send(command).IsCompletedSuccessfully)
        {
            return Results.Ok();
        }
        return Results.Problem();

    }
    [HttpPost("decline")]
    public IResult Decline(string JobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        if (JobId == null || JobId is "")
        {
            return Results.BadRequest("Job does not exist");
        }
        var command = new DeclineJobCommand(JobId);
        if (mediator.Send(command).IsCompletedSuccessfully)
        {
            return Results.Ok();
        }
        return Results.Problem();

    }
    [HttpPost("accept")]
    public async Task<IResult> Accept(string JobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        if (JobId == null || JobId is "")
        {
            return Results.BadRequest("Job does not exist");
        }
        var command = new AcceptJobCommand(JobId);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(jobResult =>
      Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerId, jobResult.Job.SellerRating, jobResult.Job.Remark, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation, jobResult.Job.SellerAddress, jobResult.Job.BuyerAddeess, jobResult.Job.IsRated, jobResult.Job.CreatedOn)), errors =>
      Results.Problem(EndpointBase.GetProblemDetails(errors)));

    }

    [HttpPost("manual")]
    public async Task<IResult> ManualCreate(CreateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var command = new CreateCommand(request.BuyerName, request.SellerName, request.SellerId, userId, request.Occupation);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerId, jobResult.Job.SellerRating, jobResult.Job.Remark, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation, jobResult.Job.SellerAddress, jobResult.Job.BuyerAddeess, jobResult.Job.IsRated, jobResult.Job.CreatedOn)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("auto")]
    public async Task<IResult> AutoCreate(AutoCreateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        var command = new AutoCreateCommand(userId, request.Occupation);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerId, jobResult.Job.SellerRating, jobResult.Job.Remark, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation, jobResult.Job.SellerAddress, jobResult.Job.BuyerAddeess, jobResult.Job.IsRated, jobResult.Job.CreatedOn)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpGet("getbyId")]
    public async Task<IResult> GetbyId(string Id)
    {
        var query = new GetbyIdQuery(Id);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(query);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerId, jobResult.Job.SellerRating, jobResult.Job.Remark, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation, jobResult.Job.SellerAddress, jobResult.Job.BuyerAddeess, jobResult.Job.IsRated, jobResult.Job.CreatedOn)), errors =>
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
    [HttpPost("UserRate")]
    public async Task<IResult> UserRate(string jobid, int rating, string remark)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || userId is "")
        {
            return Results.BadRequest("User Not Found");
        }
        repository.Remark(jobid, rating, remark);
        var job = await repository.GetById(jobid);
        var jobs = await repository.GetSellerJobs(job.SellerId);
        int totalRating = jobs.Count;
        int sumRating = jobs.Sum(j => j.SellerRating);
        double averageRating = sumRating / (double)totalRating; // Convert to double to ensure accurate division
        int roundedAverageRating = (int)Math.Round(averageRating, 0); // Round to the nearest whole number
        var profile = await profileRepository.GetById(job.SellerId);
        profile.Rating = roundedAverageRating;
        profileRepository.Update(profile);
        return Results.Ok();
    }
    [HttpGet("getUserReviews")]
    public async Task<IResult> UserReviews(string userId)
    {
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //if (userId == null || userId is "")
        //{
        //    return Results.BadRequest("User Not Found");
        //}

        // Assuming GetUserJobs returns List<Job>
        var jobs = await repository.GetUserJobs(userId);

        // Projecting the result to a new object containing only the required properties
        var result = jobs.Where(j => j.IsRated == true).Select(job => new
        {
            Name = job.BuyerName,
            Rating = job.SellerRating,
            Remark = job.Remark
        }).ToList();

        return Results.Ok(result);
    }

}
