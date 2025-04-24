using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class JsonAnalyzer : IJsonAnalyzer
{
    private readonly HttpClient _httpClient;

    public JsonAnalyzer()
    {
        _httpClient = new HttpClient();
    }

    public async Task AnalyzeJson(string jsonData)
    {
        Console.WriteLine("Отправка JSON на анализ...");
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5000/analysis", content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Результат анализа: {result}");
        }
        else
        {
            Console.WriteLine($"Ошибка при анализе JSON: {response.ReasonPhrase}");
        }
    }
}