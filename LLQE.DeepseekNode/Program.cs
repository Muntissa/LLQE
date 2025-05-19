using LLQE.Common.Interfaces;
using LLQE.Common.Services;
using LLQE.DeepseekNode.Daemons;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITopicInitializer, TopicInitializer>();

using (var tempProvider = builder.Services.BuildServiceProvider())
{
    using (var scope = tempProvider.CreateScope())
    {
        var topicInitializer = scope.ServiceProvider.GetRequiredService<ITopicInitializer>();
        await topicInitializer.EnsureTopicExistsAsync(topicInitializer.CallbackTopic);
        await topicInitializer.EnsureTopicExistsAsync(topicInitializer.ReceiveTopic);
    }
}

builder.Services.AddSingleton<IProducer, ProducerService>();
builder.Services.AddHttpClient<IRequestAI, RequestAIService>();

builder.Services.AddHostedService(provider => new DeepseekTopicConsumer(
    provider.GetRequiredService<IConfiguration>(),
    provider.GetRequiredService<ILogger<DeepseekTopicConsumer>>(), 
    provider.GetRequiredService<IProducer>(),
    provider.GetRequiredService<IRequestAI>()));

var app = builder.Build();

app.Run();
