using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PrometheusFastApiAspireSample.ApiService.Job
{
    public class JobJob : BackgroundService
    {
        private readonly ILogger<JobJob> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _messageConnection;
        private IModel? _messageChannel;
        private EventingBasicConsumer consumer;

        public JobJob(ILogger<JobJob> logger, IConfiguration config, IServiceProvider serviceProvider, IConnection? messageConnection)
        {
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queueName = "catalogEvents";

            _messageConnection = _serviceProvider.GetRequiredService<IConnection>();

            _messageChannel = _messageConnection.CreateModel();
            _messageChannel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            consumer = new EventingBasicConsumer(_messageChannel);
            consumer.Received += ProcessMessageAsync;

            _messageChannel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            consumer.Received -= ProcessMessageAsync;
            _messageChannel?.Dispose();
        }

        private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
        {

            string messagetext = Encoding.UTF8.GetString(args.Body.ToArray());
            _logger.LogInformation("All products retrieved from the catalog at {now}. Message Text: {text}", DateTime.Now, messagetext);

            var message = args.Body;
        }
    }
}