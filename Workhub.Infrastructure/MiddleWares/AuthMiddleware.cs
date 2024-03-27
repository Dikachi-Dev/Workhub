using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        var apikey = context.Request.Headers["ApiKey"].FirstOrDefault()?.Split(" ").Last();

        if (apikey != null)
        {
            if (apikey.Equals(_configuration["ApiKey"]))
            {
                await _next(context);
                return;
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }
        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("No API key was provided.");
        return;
    }
}
