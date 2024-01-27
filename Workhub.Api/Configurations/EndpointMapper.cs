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
            MapBuyerEndpoints();
            MapSellerEndpoints();
        }

        private void MapSellerEndpoints()
        {
            var seller = endpoint.MapGroup("/api/seller");
            seller.MapSellerLoginEndpoint();
        }
        private void MapBuyerEndpoints()
        {
            var buyer = endpoint.MapGroup("/api/buyer");
            buyer.MapBuyerLoginEndpoint();
            buyer.MapBuyerRegisterEndpoint();
        }
    }
}


