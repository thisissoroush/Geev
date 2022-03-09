using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;

public class ActionResultWrapperFactory : IActionResultWrapperFactory
{
    public virtual IActionResultWrapper CreateFor(ResultExecutingContext actionResult, IWebHostEnvironment env)
    {
        if (actionResult is null)
        {
            throw new ArgumentException();
        }

        // Basically, we treat ObjectResult and JsonResult in a same way
        if (actionResult.Result is ObjectResult)
        {
            return new ObjectActionResultWrapper(env);
        }
        if (actionResult.Result is JsonResult)
        {
            return new JsonActionResultWrapper();
        }

        if (actionResult.Result is EmptyResult)
        {
            return new EmptyActionResultWrapper();
        }

        return new NullActionResultWrapper();
    }
}
