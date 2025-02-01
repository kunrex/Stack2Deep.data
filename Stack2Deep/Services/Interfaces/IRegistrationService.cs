using Stack2Deep.Dal.Structures.User;

namespace Stack2Deep.Services.Interfaces;

internal interface IRegistrationService
{
    public Task<UserProfile?> TryGetProfile(long id);
    public Task<UserProfile?> TryGetProfile(string codeForceId);
    public Task<UserProfile?> TryCreateProfile(string codeForceId, string ethereumAddress, string discordId, int rating, float balance);

    public Task<bool> UpdateProfile(UserProfile profile);
    
    public Task<GroupProfile?> TryGetGroup(string groupName);
    public Task<GroupProfile?> TryCreateGroup(string groupName, DateTimeOffset start, DateTimeOffset finish);

    public Task<bool> TryGroupPlayer(string codeforcesId, string groupName);

    public Task<UserProfile[]?> TryGetProfiles(GroupProfile group);
}