using FastEndpoints;
using FastEndpoints.Swagger;
using Prometheus;
using PrometheusFastApiAspireSample.ApiService.Job;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddRedisDistributedCache("cache");
builder.AddRabbitMQClient("messaging");
builder.AddServiceDefaults();
builder.Services.AddFastEndpoints().SwaggerDocument(); ;
// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddHostedService<JobJob>();
var app = builder.Build();
app.UseFastEndpoints().UseSwaggerGen();
// Configure the HTTP request pipeline.

app.UseExceptionHandler();
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
