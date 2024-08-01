using FastEndpoints;
using System.Text;

namespace PrometheusFastApiAspireSample.ApiService.Endpoints
{
    public class RabbitEndpoint : EndpointWithoutRequest<string>
    {
        RabbitMQ.Client.IConnection connection { get; set; }
        public override void Configure()
        {
            Get("/api/rabbittest");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "catalogEvents",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var body = Encoding.UTF8.GetBytes("purum pum");

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "catalogEvents",
                                  mandatory: false,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
