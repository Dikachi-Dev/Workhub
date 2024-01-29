using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Application.Authentication.Buyer.Query;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.EndPoints
{
    public static class BuyerLoginEndpoint
    {
        public static void MapBuyerLoginEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/login", async (HttpContext context, IMediator mediator, IMapper mapper, BuyerLoginRequest request) =>
            {
                var query = mapper.Map<BuyerLoginQuery>(request);
                ErrorOr<BuyerAuthResult> loginResult = await mediator.Send(query);
                return loginResult.Match(authresult => Results.Ok(mapper.Map<BuyerLoginResponse>(authresult)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
            });
        }
    }
}
