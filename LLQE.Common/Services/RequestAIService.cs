using LLQE.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LLQE.Common.Services
{
    public class RequestAIService : IRequestAI
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken;
        private readonly ILogger<RequestAIService> _logger;

        public RequestAIService(HttpClient httpClient, IConfiguration configuration, ILogger<RequestAIService> logger)
        {
            _httpClient = httpClient;
            _apiToken = configuration["AI:ApiToken"];
            _logger = logger;
        }

        public async Task<string> SendRequestAsync(string model, string message, CancellationToken cancellationToken = default)
        {
            var requestUri = "https://bothub.chat/api/v2/openai/v1/chat/completions";

            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "user", content = message }
                },
                stream = false
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

            try
            {
                var response = await _httpClient.PostAsync(requestUri, content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                using var jsonDoc = JsonDocument.Parse(responseString);
                var reply = jsonDoc.RootElement
                                   .GetProperty("choices")[0]
                                   .GetProperty("message")
                                   .GetProperty("content")
                                   .GetString();

                return reply ?? "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending request to AI.");
                throw;
            }
        }
    }
}
