using Newtonsoft.Json;

namespace Stack2Deep.Dal.Configuration;

public static class DataConfigurationManager
{
    public static DataConfiguration DataConfiguration { get; private set; } = FromJsonFile<DataConfiguration>("..", "ignore", "config.json");
    
    public static T FromJsonFile<T>(params string[] path) where T : class
    {
        var stringPath = File.ReadAllText(Path.Combine(path));
        var result = JsonConvert.DeserializeObject<T>(stringPath);
        
        if (result == null)
        {
            Console.WriteLine($"Failed to read file at `{stringPath}`");
            return null;
        }

        return result;
    }
}