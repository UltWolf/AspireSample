using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PrometheusFastApiAspireSample.ApiService.Requests;
using PrometheusFastApiAspireSample.ApiService.Response;

namespace PrometheusFastApiAspireSample.ApiService.Endpoints
{
    [HttpPost("/my-endpoint")]
    [Authorize(Roles = "Admin,Manager")]
    [PreProcessor<MyRequestLogger<RequestTest>>]
    public class MyEndpoint : Endpoint<RequestTest, ResponseTest>
    {
        public override async Task HandleAsync(RequestTest req, CancellationToken ct)
        {
            await SendAsync(new()
            {
                FullName = req.FirstName + " " + req.LastName,
                IsOver18 = req.Age > 18
            });
        }
    }
    public class MyRequestLogger<TRequest> : IPreProcessor<TRequest>
    {
        public Task PreProcessAsync(IPreProcessorContext<TRequest> ctx, CancellationToken ct)
        {
            var logger = ctx.HttpContext.Resolve<ILogger<TRequest>>();

            logger.LogInformation(
                $"request:{ctx.Request.GetType().FullName} path: {ctx.HttpContext.Request.Path}");

            return Task.CompletedTask;
        }
    }
}
