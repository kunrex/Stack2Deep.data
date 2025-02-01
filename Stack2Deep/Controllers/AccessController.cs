using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.Controllers;

[ApiController]
[Route("forcecode/get")]
internal sealed class AccessController : BaseController
{
    private IRegistrationService _registration;

    public AccessController(IRegistrationService registrationService) : base()
    {
        _registration = registrationService;
    }

    [HttpGet("group")]
    public async Task<ActionResult> TryGetGroup([FromQuery] string groupName)
    {
        try
        {
            var group = await _registration.TryGetGroup(groupName);
            if(group == null)
                return FromContent($"Group: {group} does not exist.", 404);

            var users = await _registration.TryGetProfiles(group);
            if(users == null)
                return FromContent($"Failed to find profiles for group: {groupName}", 500);
            
            return FromContent(JsonSerializer.Serialize(users.Select(x => x.CodeforcesId)), 200);
        }
        catch
        {
            return InternalError();
        }
    }
    
    [HttpGet("user")]
    public async Task<ActionResult> TryGetUser([FromQuery] string codeForcesId)
    {
        try
        {
            var profile = await _registration.TryGetProfile(codeForcesId);
            if (profile == null)
                return FailedUserFetchResult(codeForcesId);

            return FromContent(JsonSerializer.Serialize(profile), 200);
        }
        catch
        {
            return InternalError();
        }
    }
}