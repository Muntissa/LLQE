using System;
using System.Threading.Tasks;

public class TaskDistributor : ITaskDistributor
{
    private readonly IJsonAnalyzer _jsonAnalyzer;
    private readonly ILogger _logger;

    public TaskDistributor(IJsonAnalyzer jsonAnalyzer, ILogger logger)
    {
        _jsonAnalyzer = jsonAnalyzer;
        _logger = logger;
    }

    public async Task DistributeTasks(string jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Console.WriteLine("Нет данных для обработки.");
            return;
        }

        Console.WriteLine("Распределение задач...");
        await _jsonAnalyzer.AnalyzeJson(jsonData);
        _logger.LogJsonProcessing(jsonData);
    }
} 