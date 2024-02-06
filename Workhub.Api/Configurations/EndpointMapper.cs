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

            MapUserAuthEndpoints();
        }

        private void MapUserAuthEndpoints()
        {
            var buyer = endpoint.MapGroup("/api/auth");
            buyer.MapLoginEndpoint();
            buyer.MapRegisterEndpoint();
        }
    }
}


