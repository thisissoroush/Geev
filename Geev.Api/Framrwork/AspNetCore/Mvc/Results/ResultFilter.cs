using Microsoft.AspNetCore.Mvc.Filters;
using Geev.Api.Framrwork.AspNetCore.Config;
using Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;
using Geev.Api.Framrwork.Reflection;

namespace Geev.Api.Framrwork.AspNetCore.Mvc.Results;

public class ResultFilter : IResultFilter
{
    private readonly IAspnetCoreConfiguration _configuration;
    private readonly IActionResultWrapperFactory _actionResultWrapperFactory;
    private readonly IWebHostEnvironment _env;

    public ResultFilter(IAspnetCoreConfiguration configuration, IActionResultWrapperFactory actionResultWrapperFactory, IWebHostEnvironment env)
    {
        _actionResultWrapperFactory = actionResultWrapperFactory;
        _configuration = configuration;
        _env = env;
    }

    public virtual void OnResultExecuting(ResultExecutingContext context)
    {
        // TODO: Checks whether if must Wrap the result or not - We must introduce a config for this
        var methodInfo = context.ActionDescriptor.GetMethodInfo();

        //var clientCacheAttribute = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
        //    methodInfo,
        //    _configuration.DefaultClientCacheAttribute
        //);

        //clientCacheAttribute?.Apply(context);

        var wrapResultAttribute =
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                methodInfo,
                _configuration.DefaultWrapResultAttribute
            );

        if (!wrapResultAttribute.WrapOnSuccess)
        {
            return;
        }

        _actionResultWrapperFactory.CreateFor(context, _env).Wrap(context);
    }

    public virtual void OnResultExecuted(ResultExecutedContext context)
    {
        // Nothing
    }
}
