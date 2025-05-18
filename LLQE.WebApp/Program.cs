using LLQE.Common.Entities;
using LLQE.Common.Interfaces;
using LLQE.Common.Services;
using LLQE.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<NodeMessagesStore>();
builder.Services.AddSingleton<MultiTopicProducer>();
builder.Services.AddHostedService<MultiTopicConsumer>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
