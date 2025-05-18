using LLQE.Common.Interfaces;
using LLQE.Common.Services;
using LLQE.DeepseekNode.Daemons;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProducer, ProducerService>();
builder.Services.AddHttpClient<IRequestAI, RequestAIService>();

builder.Services.AddHostedService(provider => new DeepseekTopicConsumer(
    provider.GetRequiredService<IConfiguration>(),
    provider.GetRequiredService<ILogger<DeepseekTopicConsumer>>(), 
    provider.GetRequiredService<IProducer>(),
    provider.GetRequiredService<IRequestAI>()));

var app = builder.Build();
app.Run();
