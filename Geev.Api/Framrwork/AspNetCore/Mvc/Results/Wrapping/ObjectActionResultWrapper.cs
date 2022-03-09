using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Geev.Api.Framrwork.Models;
using Geev.Api.Framrwork.Tracing;

namespace Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;

public class ObjectActionResultWrapper : IActionResultWrapper
{
    private readonly IWebHostEnvironment _env;

    public ObjectActionResultWrapper(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void Wrap(ResultExecutingContext actionResult)
    {
        if (actionResult.Result is not ObjectResult objectResult)
        {
            throw new ArgumentException($"{nameof(actionResult)} should be ObjectResult!");
        }

        if (objectResult.Value is not GeevApiResponseBase)
        {
            var response = new GeevApiResponse(objectResult.Value)
            {

                // TODO: move to configurations/options
                //if (_env.IsDevelopment() || _env.IsStaging())
                __traceId = TracingExtensions.GetCompactTraceId()
            };

            actionResult.Result = new ObjectResult(response)
            {
                StatusCode = objectResult.StatusCode
            };
        }
    }
}
