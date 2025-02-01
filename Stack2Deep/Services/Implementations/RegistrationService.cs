using Microsoft.EntityFrameworkCore;

using Stack2Deep.Dal;
using Stack2Deep.Dal.Structures.User;
using Stack2Deep.Dal.Structures.Contexts;

using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.Services.Implementations;

internal sealed class RegistrationService : DataBaseService, IRegistrationService
{
    private static string EncodeGroup(in IEnumerable<UserProfile> profiles)
    {
        return string.Join('-', profiles.Select(x => x.CodeforcesId));
    }
    
    public RegistrationService(DataContext dataContext) : base(dataContext)
    { }
    
    public async Task<UserProfile?> TryGetProfile(long id)
    {
        return await context.Profiles.FindAsync(id);
    }
    
    public async Task<UserProfile?> TryGetProfile(string codeforcesId)
    {
        return await context.Profiles.FirstOrDefaultAsync(x => x.CodeforcesId == codeforcesId);
    }

    public async Task<UserProfile?> TryCreateProfile(string codeforcesId, string ethereumAddress, string discordUsername, int rating, float balance)
    {
        var profile = await context.Profiles.FirstOrDefaultAsync(x => x.CodeforcesId == codeforcesId);
        
        if (profile == null)
        {
            profile = new UserProfile(codeforcesId, ethereumAddress, discordUsername, rating, balance);
            await AddEntity(profile);
        }

        return profile;
    }

    public async Task<bool> UpdateProfile(UserProfile profile)
    {
        return await UpdateEntity(profile);
    }
    
    public async Task<GroupProfile?> TryGetGroup(string groupName)
    {
        return await context.Groups.FirstOrDefaultAsync(x => x.GroupName == groupName);
    }

    public async Task<GroupProfile?> TryCreateGroup(string groupName, DateTimeOffset start, DateTimeOffset finish)
    {
        var group = await TryGetGroup(groupName);
        if (group != null)
            return group;

        group = new GroupProfile(groupName, start, finish);
        await AddEntity(group);

        return group;
    }

    public async Task<bool> TryGroupPlayer(string codeforcesId, string groupName)
    {
        var profile = await TryGetProfile(codeforcesId);
        if (profile == null)
            return false;

        var group = await TryGetGroup(groupName);
        if (group == null)
            return false;
        
        var result = await context.Contexts
            .Where(ctx => ctx.GroupProfileId == group.Id)
            .Select(ctx => ctx.UserProfileId)
            .ToArrayAsync();

        if (result.Contains(profile.Id))
            return true;
        
        return await AddEntity(new UserGroupContext(group.Id, profile.Id));
    }

    public async Task<UserProfile[]?> TryGetProfiles(GroupProfile group)
    {
        var result = await context.Contexts
            .Where(ctx => ctx.GroupProfileId == group.Id)
            .Select(ctx => ctx.UserProfileId)
            .ToArrayAsync();

        var users = new UserProfile[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            var user = await TryGetProfile(result[i]);
            if (user == null)
                return null;
            
            users[i] = user;
        }

        return users;
    }

    public async Task<bool> DeleteGroup(string groupName)
    {
        var group = await TryGetGroup(groupName);
        if (group == null)
            return false;

        var contexts = context.Contexts.Where(x => x.GroupProfileId == group.Id);
        foreach (var ctx in contexts)
            await RemoveEntity(ctx);

        return await RemoveEntity(group);
    }
}