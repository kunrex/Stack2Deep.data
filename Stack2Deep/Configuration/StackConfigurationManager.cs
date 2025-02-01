using Newtonsoft.Json;

namespace Stack2Deep.Configuration;

public static class StackConfigurationManager
{
    public static Configuration Configuration { get; private set; } = FromJsonFile<Configuration>("ignore", "config.json");
    private static T FromJsonFile<T>(params string[] path) where T : class
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