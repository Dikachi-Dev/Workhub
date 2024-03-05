using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Workhub.Api.EndPoints;
using Workhub.Application.Jobber.Command;
using Workhub.Application.Jobber.Common;
using Workhub.Application.Jobber.Query;
using Workhub.Contracts.Job;

namespace Workhub.Api.Controllers;

[Route("api/job")]
[ApiController]
public class JobController
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public JobController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost("manual")]
    public async Task<IResult> ManualCreate(CreateRequest request)
    {
        var command = mapper.Map<CreateCommand>(request);
        ErrorOr<GetJobResult> jobResult = await mediator.Send(command);
        return jobResult.Match(jobResult =>
        Results.Ok(new JobResponse(jobResult.Job.Id, jobResult.Job.BuyerName, jobResult.Job.SellerName, jobResult.Job.SellerRating, jobResult.Job.BuyerRating, jobResult.Job.Status, jobResult.Job.BuyerId, jobResult.Job.Occupation)), errors =>
        Results.Problem(EndpointBase.GetProblemDetails(errors)));
    }

    [HttpPost("auto")]
    public async Task<IResult> AutoCreate(AutoCreateRequest request)
    {
        var command = mapper.Map<AutoCreateCommand>(request);
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
}
