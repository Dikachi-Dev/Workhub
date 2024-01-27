using ErrorOr;
using MapsterMapper;
using MediatR;
using Workhub.Application.Authentication.Seller.Common;
using Workhub.Application.Authentication.Seller.Query;
using Workhub.Contracts.Authentication;

namespace Workhub.Api.EndPoints;
public static class SellerLoginEndpoint
{
    public static void MapSellerLoginEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/sellerlogin", async (IMediator mediator, IMapper mapper, SellerLoginRequest request) =>
        {
            var query = mapper.Map<SellerLoginQuery>(request);
            ErrorOr<SellerAuthResult> loginResult = await mediator.Send(query);
            return loginResult.Match(authresult => Results.Ok(mapper.Map<SellerLoginResponse>(authresult)), errors => Results.Problem(EndpointBase.GetProblemDetails(errors)));
        });
    }
}


