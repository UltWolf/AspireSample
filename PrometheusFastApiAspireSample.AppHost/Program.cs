var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache");
var rabbit = builder.AddRabbitMQ("messaging");
var apiService = builder.AddProject<Projects.PrometheusFastApiAspireSample_ApiService>("apiservice")
    .WithReference(rabbit)
    .WithReference(redis);
builder.AddProject<Projects.PrometheusFastApiAspireSample_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
     .WithReference(rabbit)
    .WithReference(redis);


builder.Build().Run();
