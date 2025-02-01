namespace Stack2Deep.Dal.Structures.User;

public sealed class UserProfile : Item
{
    public string CodeforcesId { get; set; } = string.Empty;
    public int CodeForcesRating { get; set; }
    
    public string EthereumAddress { get; set; } = string.Empty;
    public string DiscordUsername { get; set; } = string.Empty;
    
    public float EthereumBalance { get; set; }

    public UserProfile()
    { }
    
    public UserProfile(string codeforcesId, string ethereumAddress, string discordUsername, int codeForcesRating, float ethereumBalance)
    {
        CodeforcesId = codeforcesId;
        CodeForcesRating = codeForcesRating;
        
        EthereumAddress = ethereumAddress;
        DiscordUsername = discordUsername;

        EthereumBalance = ethereumBalance;
    }
}