using FastEndpoints;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace PrometheusFastApiAspireSample.ApiService.Endpoints
{
    public class CachedEndpoint : EndpointWithoutRequest<string>
    {
        public IDistributedCache cache { get; set; }
        public override void Configure()
        {
            Get("/api/cached");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var cachedTime = await cache.GetAsync("time");

            if (cachedTime is null)
            {
                Response = DateTime.Now.ToString();

                await cache.SetAsync("time", Encoding.UTF8.GetBytes(DateTime.Now.ToString()), new()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(20)
                }); ;


            }
            else
            {
                Response = Encoding.UTF8.GetString(cachedTime);
            }
        }
    }
}
