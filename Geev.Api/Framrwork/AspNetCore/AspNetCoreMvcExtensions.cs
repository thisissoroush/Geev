using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Geev.Core.Exceptions;
using System.Reflection;

namespace Geev.Api.Framrwork.AspNetCore;

/// <summary>
/// Helper and Utility methods
/// </summary>
public static class AspNetCoreMvcExtensions
{
    public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
    {
        if (actionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            throw new GeevException(
                $"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");
        }

        return controllerActionDescriptor;
    }

    public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor.AsControllerActionDescriptor().MethodInfo;
    }

    public static bool IsObjectResult(Type returnType)
    {
        //Get the actual return type (unwrap Task)
        if (returnType == typeof(Task))
        {
            returnType = typeof(void);
        }
        else if (returnType.GetTypeInfo().IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            returnType = returnType.GenericTypeArguments[0];
        }

        return typeof(IActionResult).GetTypeInfo().IsAssignableFrom(returnType)
            ? typeof(JsonResult).GetTypeInfo().IsAssignableFrom(returnType) || typeof(ObjectResult).GetTypeInfo().IsAssignableFrom(returnType)
            : true;
    }
}
