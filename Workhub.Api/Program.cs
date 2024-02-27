using Workhub.Api.Configurations;
using Workhub.Application;
using Workhub.Infrastructure;
//using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddWorkhubApiServices();

builder.Services.AddSignalR();
builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Add authentication and authorization middleware before endpoints
app.UseAuthentication();
app.UseMiddleware<AuthMiddleware>();
app.UseAuthorization();


// Map endpoints
var endpointMapper = new EndpointMapper(app);
endpointMapper.MapAllEndpoints();
//app.UseEndpoints(endpoint =>
//{
//    EndpointMapper endpointMapper = new EndpointMapper(endpoint);
//    endpointMapper.MapAllEndpoints();
//});

app.Run();