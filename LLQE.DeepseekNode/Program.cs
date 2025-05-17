using LLQE.Common.Interfaces;
using LLQE.Common.Services;
using LLQE.DeepseekNode.Daemons;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IRequestAI, RequestAIService>();

builder.Services.AddHostedService(provider => new DeepseekTopicConsumer("DeepseekPrompts", "Deepseek",
    provider.GetRequiredService<ILogger<DeepseekTopicConsumer>>(), 
    provider.GetRequiredService<IRequestAI>()));

var app = builder.Build();
app.Run();
