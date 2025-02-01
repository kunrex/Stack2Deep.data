using Microsoft.AspNetCore.Mvc;

using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.Controllers;

[ApiController]
[Route("forcecode/codeforces")]
internal sealed class CodeForcesController : BaseController
{
    private readonly IRegistrationService _registration;
    private readonly ICodeForcesService _codeForces;
    
    public CodeForcesController(IRegistrationService registrationService, ICodeForcesService codeForcesService) : base()
    {
        _registration = registrationService;
        _codeForces = codeForcesService;
    }

    [HttpGet("difference")]
    public async Task<ActionResult<int?>> TryGetDifference([FromQuery] string codeforcesId)
    {
        try
        {
            var profile = await _registration.TryGetProfile(codeforcesId);
            if (profile == null)
                return FromContent($"Failed to get profile of user: {codeforcesId}", 404);
            
            var rating = await _codeForces.GetRating(codeforcesId);
            if (!rating.Item1)
                return FromContent($"Failed to get rating of user: {codeforcesId}", 404);

            profile.CodeForcesRating = rating.Item2;
            await _registration.UpdateProfile(profile);
            return new JsonResult( new { message = rating.Item2 - profile.CodeForcesRating, code = 200 });
        }
        catch 
        {
            return InternalError();
        }
    }
}