using System;
using System.IO;

public class JsonLoader : IJsonLoader
{
    public string LoadJson(string filePath)
    {
        try
        {
            Console.WriteLine($"Загрузка JSON из {filePath}");
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке JSON: {ex.Message}");
            return null;
        }
    }
} 