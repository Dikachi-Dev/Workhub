using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Authentication.Buyer.Commands;
using Workhub.Application.Authentication.Buyer.Common;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.EndPoints
{
    public static class BuyerRegisterEndpoint
    {
        public static void MapBuyerRegisterEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/register", async (IMediator mediator, IMapper mapper, BuyerRegisterRequest request) =>
            {
                var command = mapper.Map<RegisterBuyerCommand>(request);
                ErrorOr<BuyerAuthResult> registerResult = await mediator.Send(command);
                return registerResult
                .Match(authResult => Results.Ok(mapper.Map<BuyerLoginResponse>(authResult)),
                errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
            });
        }
    }
}
