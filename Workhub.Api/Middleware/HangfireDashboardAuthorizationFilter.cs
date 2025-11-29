using Hangfire.Dashboard;

namespace Workhub.Api.Middleware;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        
        // Allow access if ApiKey header is present and valid
        var apiKey = httpContext.Request.Headers["ApiKey"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(apiKey))
        {
            return false;
        }

        var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedApiKey = configuration["ApiKey"];

        return apiKey.Equals(expectedApiKey);
    }
}
