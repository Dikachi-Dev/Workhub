using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Profiless.Common;
using Workhub.Application.Profiless.Query;
using Workhub.Contracts.Profileing;

namespace Workhub.Api.EndPoints.ExchangeEndpoints;

public static class GetAllSellersEndpoint
{
    public static void MapGetAllSellersEndpoints(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/allsellers", async (IMediator mediator, IMapper mapper, GetAllRequest request) =>
        {
            var query = mapper.Map<GetAllQuery>(request);
            ErrorOr<GetAllResult> results = await mediator.Send(query);
            return results.Match(allresults => Results.Ok(mapper.Map<GetAllResponse>(allresults)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });
    }
}
