using Newtonsoft.Json;

using Stack2Deep.Dal.Configuration;

namespace Stack2Deep.Configuration;

public static class StackConfigurationManager
{
    public static StackConfiguration Configuration { get; private set; } = DataConfigurationManager.FromJsonFile<StackConfiguration>("..", "ignore", "config.json");
}