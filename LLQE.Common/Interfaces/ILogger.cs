using System;
using System.Threading.Tasks;

public interface ILogger
{
    void LogJsonProcessing(string jsonData);
    Task MonitorPerformance(Func<Task> action);
}