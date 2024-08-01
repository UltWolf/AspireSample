using FastEndpoints;
using PrometheusFastApiAspireSample.ApiService.Mapper;
using PrometheusFastApiAspireSample.ApiService.Requests;
using PrometheusFastApiAspireSample.ApiService.Response;

namespace PrometheusFastApiAspireSample.ApiService.Endpoints
{
    public class SavePerson : Endpoint<RequestPersonTest, ResponsePersonTest, PersonMapper>
    {
        public override void Configure()
        {
            Put("/api/person");
        }

        public override Task HandleAsync(RequestPersonTest r, CancellationToken c)
        {
            Person entity = Map.ToEntity(r);
            Response = Map.FromEntity(entity);
            return SendAsync(Response);
        }
    }
}
