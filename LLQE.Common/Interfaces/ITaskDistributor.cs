using System.Threading.Tasks;

public interface ITaskDistributor
{
    Task DistributeTasks(string jsonData);
} 