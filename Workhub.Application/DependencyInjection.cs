using Microsoft.Extensions.DependencyInjection;

namespace Workhub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register other services


        // Register MediatR handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly));

        return services;
    }


}
