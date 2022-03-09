using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Geev.Api.Framrwork.Models;
using Geev.Api.Framrwork.Tracing;

namespace Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;

/// <summary>
/// TODO: Not tested JsonResult but it should work
/// </summary>
public class JsonActionResultWrapper : IActionResultWrapper
{
    public void Wrap(ResultExecutingContext actionResult)
    {
        if (actionResult.Result is not JsonResult jsonResult)
        {
            throw new ArgumentException($"{nameof(actionResult)} should be JsonResult!");
        }

        if (jsonResult.Value is not GeevApiResponseBase)
        {
            var response = new GeevApiResponse(jsonResult.Value)
            {

                // TODO: move to configurations/options
                //if (_env.IsDevelopment() || _env.IsStaging())
                __traceId = TracingExtensions.GetCompactTraceId()
            };

            actionResult.Result = new ObjectResult(response)
            {
                StatusCode = jsonResult.StatusCode
            };
        }
    }
}
