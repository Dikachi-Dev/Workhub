using Hangfire.Dashboard;

namespace Workhub.Api.Middleware;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
        
        // Get credentials from configuration
        var expectedUser = configuration["Hangfire:User"];
        var expectedPass = configuration["Hangfire:Pass"];
        
        // Check for Basic Authentication header
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        
        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var credentials = decodedCredentials.Split(':', 2);
            
            if (credentials.Length == 2)
            {
                var username = credentials[0];
                var password = credentials[1];
                
                if (username == expectedUser && password == expectedPass)
                {
                    return true;
                }
            }
        }
        
        // Request Basic Authentication
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        return false;
    }
}
