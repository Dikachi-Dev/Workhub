using Workhub.Api.EndPoints;
using Workhub.Infrastructure.Notification;

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
            endpoint.MapHub<NotificationHub>("/noticehub");

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


