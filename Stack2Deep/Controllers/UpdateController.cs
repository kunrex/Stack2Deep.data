using Microsoft.AspNetCore.Mvc;
using Stack2Deep.Configuration;
using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.Controllers;

[ApiController]
[Route("forcecode/update")]
internal sealed class UpdateController : BaseController
{
    private IRegistrationService _registration;

    public UpdateController(IRegistrationService registrationService)
    {
        _registration = registrationService;
    }

    [HttpPost("user")]
    public async Task<ActionResult> UpdateUser([FromQuery] string codeForcesId, [FromQuery] float newBalance)
    {
        try
        {
            var profile = await _registration.TryGetProfile(codeForcesId);
            if (profile == null)
                return FailedUserFetchResult(codeForcesId);

            profile.EthereumBalance = newBalance;
            await _registration.UpdateProfile(profile);
            return FromContent(true.ToString(), 200);
        }
        catch
        {
            return InternalError();
        }
    }

    [HttpGet("completed")]
    public async Task<ActionResult> CheckComplete([FromQuery] string groupName)
    {
        try
        {
            var group = await _registration.TryGetGroup(groupName);
            if (group == null)
                return FromContent($"Failed to get group: {groupName}", 404);

            var result = group.FinishDate > DateTimeOffset.UtcNow;
            if (result)
            {
                if (DateTimeOffset.UtcNow > group.FinishDate + TimeSpan.FromDays(StackConfigurationManager.Configuration.DayGap));
                    await _registration.DeleteGroup(groupName);
            }
            
            return FromContent(result.ToString(), 200);
        }
        catch
        {
            return InternalError();
        }
    }
}