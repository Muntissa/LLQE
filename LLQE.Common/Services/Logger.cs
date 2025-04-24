using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class Logger : ILogger
{
    public void LogJsonProcessing(string jsonData)
    {
        Console.WriteLine("Логирование обработки JSON...");
        // Логика логирования
    }

    public async Task MonitorPerformance(Func<Task> action)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        await action();
        stopwatch.Stop();
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
    }
}