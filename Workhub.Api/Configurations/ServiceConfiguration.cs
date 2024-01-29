using MapsterMapper;
using MediatR;

namespace Workhub.Api.Configurations
{
    // Inside Workhub.Api.Configurations namespace
    public static class ServiceConfigurations
    {
        public static IServiceCollection AddWorkhubApiServices(this IServiceCollection services)
        {
            // Add your services here
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IMapper, Mapper>();
            services.AddDatabaseSetup();
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Workhub.Application.AssemblyReference.Assembly));




            // Add other services as needed


            return services;


        }
    }

}

