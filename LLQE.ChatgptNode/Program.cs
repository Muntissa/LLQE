using LLQE.ChatgptNode.Daemons;
using LLQE.Common.Interfaces;
using LLQE.Common.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5002");

builder.Services.AddScoped<ITopicInitializer, TopicInitializer>();

builder.Services.AddSingleton<IProducer, ProducerService>();
builder.Services.AddHttpClient<IRequestAI, RequestAIService>();

builder.Services.AddHostedService(provider => new ChathptTopicConsumer(
    provider.GetRequiredService<IConfiguration>(),
    provider.GetRequiredService<ILogger<ChathptTopicConsumer>>(),
    provider.GetRequiredService<IProducer>(),
    provider.GetRequiredService<IRequestAI>()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var topicInitializer = scope.ServiceProvider.GetRequiredService<ITopicInitializer>();
    await topicInitializer.EnsureTopicExistsAsync(topicInitializer.CallbackTopic);
    await topicInitializer.EnsureTopicExistsAsync(topicInitializer.ReceiveTopic);
}

app.Run();

