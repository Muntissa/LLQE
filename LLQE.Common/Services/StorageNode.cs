using System;
using System.IO;

public class StorageNode : IStorageNode
{
    public void StoreJson(string jsonData)
    {
        string filePath = "stored_json.json";
        File.WriteAllText(filePath, jsonData);
        Console.WriteLine($"JSON сохранен в {filePath}");
    }
} 