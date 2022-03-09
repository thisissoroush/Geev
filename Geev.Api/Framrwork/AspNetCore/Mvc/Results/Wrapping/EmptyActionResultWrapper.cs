using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Geev.Api.Framrwork.Models;

namespace Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;

public class EmptyActionResultWrapper : IActionResultWrapper
{
    public void Wrap(ResultExecutingContext actionResult)
    {
        actionResult.Result = new ObjectResult(new GeevApiResponse())
        {
            StatusCode = actionResult.HttpContext.Response.StatusCode
        };
    }
}
