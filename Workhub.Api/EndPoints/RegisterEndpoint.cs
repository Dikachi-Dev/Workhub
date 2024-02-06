using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Authentication.Seller.Commands;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.EndPoints
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/register", async (IMediator mediator, IMapper mapper, RegisterRequest request) =>
            {
                var command = mapper.Map<RegisterCommand>(request);
                ErrorOr<AuthResult> registerResult = await mediator.Send(command);
                return registerResult
                .Match(authResult => Results.Ok(mapper.Map<LoginResponse>(authResult)),
                errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
            });
        }
    }
}
