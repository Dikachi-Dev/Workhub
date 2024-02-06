using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Authentication.Seller.Query;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.EndPoints;
public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/login", async (IMediator mediator, IMapper mapper, LoginRequest request) =>
        {
            var query = mapper.Map<LoginQuery>(request);
            ErrorOr<AuthResult> loginResult = await mediator.Send(query);
            return loginResult.Match(authresult => Results.Ok(mapper.Map<LoginResponse>(authresult)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });
    }
}


