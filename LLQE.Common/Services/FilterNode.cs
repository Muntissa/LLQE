using System;
using Newtonsoft.Json.Linq;

public class FilterNode : IFilterNode
{
    public string FilterJson(string jsonData)
    {
        Console.WriteLine("Фильтрация JSON данных...");
        var jsonObject = JObject.Parse(jsonData);
        // Пример фильтрации: удаление всех свойств с пустыми значениями
        foreach (var property in jsonObject.Properties())
        {
            if (property.Value.Type == JTokenType.Null || property.Value.ToString() == string.Empty)
            {
                property.Remove();
            }
        }
        return jsonObject.ToString();
    }
}