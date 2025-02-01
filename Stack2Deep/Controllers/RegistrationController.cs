using Microsoft.AspNetCore.Mvc;

using Stack2Deep.Dal.Structures.User;

using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.Controllers;

[ApiController]
[Route("forcecode/registration")]
internal sealed class RegistrationController : BaseController
{
    private readonly IRegistrationService _registration;
    private readonly ICodeForcesService _codeForces;

    public RegistrationController(IRegistrationService registrationService, ICodeForcesService codeForcesService) : base()
    {
        _registration = registrationService;
        _codeForces = codeForcesService;
    }
    
    [HttpPost("user")]
    public async Task<ActionResult> TryRegisterUser([FromQuery] string codeforcesId, [FromQuery] string ethereumAddress, [FromQuery] string discordUsername, [FromQuery] float balance)
    {
        try
        {
            var rating = await _codeForces.GetRating(codeforcesId);
            if (!rating.Item1)
                return FromContent($"Failed to get codeforces rating for the handle: {codeforcesId}.", 400);
            
            if(await _registration.TryGetProfile(codeforcesId) != null)
                return FromContent($"User: {codeforcesId} already exists.", 400);
            
            await _registration.TryCreateProfile(codeforcesId, ethereumAddress, discordUsername, rating.Item2, balance);
            return new JsonResult(new { message = true, code = 200 });
        }
        catch
        {
            return InternalError();
        }
    }
    
    
    [HttpPost("join")]
    public async Task<ActionResult> TryRegisterPlayer([FromQuery] string groupName, [FromQuery] string codeForcesId)
    {
        try
        {
            if (await _registration.TryGetGroup(groupName) != null)
                return FromContent($"Group: {groupName} already exists", 404);

            if (await _registration.TryGetProfile(codeForcesId) != null)
                return FailedUserFetchResult(codeForcesId);
            
            if(await _registration.TryGroupPlayer(groupName, codeForcesId))
                return FromContent(true.ToString(), 200);

            return FromContent($"Failed to register player: {codeForcesId} to group: {groupName}", 500);
        }
        catch 
        {
            return InternalError();
        }
    }
    
    [HttpPost("group")]
    public async Task<ActionResult> TryRegisterGroup([FromQuery] string groupName, [FromQuery] string creatorCodeForcesId, [FromQuery] DateTimeOffset? finishDate)
    {
        try
        {
            if (finishDate == null)
                return FromContent($"Failed to read finish date", 404);
                
            if (await _registration.TryGetGroup(groupName) != null)
                return FromContent($"Group: {groupName} already exists", 404);

            if (await _registration.TryGetProfile(creatorCodeForcesId) != null)
                return FailedUserFetchResult(creatorCodeForcesId);

            await _registration.TryCreateGroup(groupName, DateTimeOffset.UtcNow, finishDate.Value);
            
            if(await _registration.TryGroupPlayer(groupName, creatorCodeForcesId))
                return FromContent(true.ToString(), 200);

            return FromContent($"Failed to create group: {groupName}", 500);
        }
        catch
        {
            return InternalError();
        }
    }
}