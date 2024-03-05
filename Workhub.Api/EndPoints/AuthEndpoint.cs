using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Authentication.Seller.Commands;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Authentication.Seller.Query;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.EndPoints
{
    public static class AuthEndpoint
    {
        public static void MapAuthEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/register", async (IMediator mediator, IMapper mapper, RegisterRequest request) =>
            {
                var command = mapper.Map<RegisterCommand>(request);
                ErrorOr<AuthResult> registerResult = await mediator.Send(command);
                return registerResult
                .Match(authResult => Results.Ok(mapper.Map<LoginResponse>(authResult)),
                errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
            });

            endpoint.MapPost("/login", async (IMediator mediator, IMapper mapper, LoginRequest request) =>
            {
                var query = mapper.Map<LoginQuery>(request);
                ErrorOr<AuthResult> loginResult = await mediator.Send(query);
                return loginResult.Match(authresult => Results.Ok(mapper.Map<LoginResponse>(authresult)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
            });
        }
    }
}
