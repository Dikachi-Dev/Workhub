using Workhub.Api.EndPoints;

namespace Workhub.Api.Configurations
{
    public class EndpointMapper
    {
        private readonly IEndpointRouteBuilder endpoint;

        public EndpointMapper(IEndpointRouteBuilder endpoint)
        {
            this.endpoint = endpoint;
        }

        public void MapAllEndpoints()
        {
            SellerLoginEndpoint.MapSellerLoginEndpoint(endpoint);
            BuyerLoginEndpoint.MapBuyerLoginEndpoint(endpoint);
        }
    }
}
