using System.Text.Json;
using System.Text.Json.Nodes;
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

            return new JsonResult( new { message = users.Select(x => x.CodeforcesId).ToArray(), code = 200 });
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

            return new JsonResult( new { message = profile, code = 200 });
        }
        catch
        {
            return InternalError();
        }
    }
}