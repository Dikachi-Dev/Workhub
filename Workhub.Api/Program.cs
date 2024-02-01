using Workhub.Api.Configurations;
using Workhub.Application;
using Workhub.Infrastructure;
//using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWorkhubApiServices();
builder.Services.AddApplication()
    .AddInfrastructure();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
//app.UseAuthorization();
app.UseEndpoints(endpoint =>
{
    EndpointMapper endpointMapper = new EndpointMapper(endpoint);
    endpointMapper.MapAllEndpoints();
});



app.Run();
