// Program.cs
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LLQE.Common
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IJsonLoader, JsonLoader>()
                .AddSingleton<ITaskDistributor, TaskDistributor>()
                .AddSingleton<IJsonAnalyzer, JsonAnalyzer>()
                .AddSingleton<ILogger, Logger>()
                .AddSingleton<IStorageNode, StorageNode>()
                .AddSingleton<IFilterNode, FilterNode>()
                .BuildServiceProvider();

            var jsonLoader = serviceProvider.GetService<IJsonLoader>();
            var taskDistributor = serviceProvider.GetService<ITaskDistributor>();
            var logger = serviceProvider.GetService<ILogger>();
            var storageNode = serviceProvider.GetService<IStorageNode>();
            var filterNode = serviceProvider.GetService<IFilterNode>();

            Console.WriteLine("Добро пожаловать в распределенную систему обработки JSON!");

            string jsonFilePath = "C:/TestDataJSON/testjson.json";
            string jsonData = jsonLoader.LoadJson(jsonFilePath);

            await logger.MonitorPerformance(async () =>
            {
                string filteredJson = filterNode.FilterJson(jsonData);
                await taskDistributor.DistributeTasks(filteredJson);
                storageNode.StoreJson(filteredJson);
            });

            Console.WriteLine("Обработка завершена.");
        }
    }
}