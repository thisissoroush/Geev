using Microsoft.AspNetCore.Mvc.Filters;

namespace Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;

public interface IActionResultWrapper
{
    void Wrap(ResultExecutingContext actionResult);
}
