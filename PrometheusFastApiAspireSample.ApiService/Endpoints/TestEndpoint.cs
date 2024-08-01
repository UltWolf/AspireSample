using FastEndpoints;
using PrometheusFastApiAspireSample.ApiService.Requests;
using PrometheusFastApiAspireSample.ApiService.Response;

namespace PrometheusFastApiAspireSample.ApiService.Endpoints
{
    public class TestEndpoint : Endpoint<RequestTest, ResponseTest>
        {
            public override void Configure()
            {
                Post("/api/user/create");
                AllowAnonymous();
            }

            public override async Task HandleAsync(RequestTest req, CancellationToken ct)
            {
                await SendAsync(new()
                {
                    FullName = req.FirstName + " " + req.LastName,
                    IsOver18 = req.Age > 18
                });
            }
        } 
}
