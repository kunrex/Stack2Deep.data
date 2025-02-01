using Microsoft.AspNetCore.Mvc;

namespace Stack2Deep.Controllers;

internal abstract class BaseController
{
    protected JsonResult InternalError()
    {
        return FromContent($"Internal Server Error.", 500);
    }

    protected JsonResult FailedUserFetchResult(string id)
    {
        return FromContent($"Failed to get user: {id}. Make sure you're  registered ForceCode user.", 404);
    }
    
    protected JsonResult FromContent(in string message, in int code)
    {
        return new JsonResult(new
        {
            message = message,
            code = code
        });
    }
}