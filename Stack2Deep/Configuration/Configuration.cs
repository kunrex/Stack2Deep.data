namespace Stack2Deep.Configuration;

public sealed class Configuration
{
    public string Host { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string Database { get; set; }
    
    public int CallGap { get; set; }
}