using System.Diagnostics;
using Geev.Api.Framrwork.Tracing.Diagnostic;

namespace Geev.Api.Framrwork.Tracing.Http;

public static class HttpActivityManager
{
    /// <summary>
    /// Add activity data to http response header for getting this activity context in next calls
    /// </summary>
    /// <param name="responseHeader">Http Response header</param>
    public static void AddActivityToResponseHeader(IHeaderDictionary responseHeader)
    {
        try
        {
            IfEnabled()?.AddActivityToResponseHeader(responseHeader);
        }
        catch (Exception exp)
        {
            //ignored
        }
    }

    /// <summary>
    /// Start new activity based on trace information on request headers, if trace information
    /// be include in request headers, new activity will set as child of 
    /// </summary>
    /// <param name="requestHeader"></param>
    /// <returns></returns>
    public static Activity? SetParentActivityFromRequestHeader(IHeaderDictionary requestHeader)
    {
        try
        {
            return IfEnabled()?.SetParentActivityFromRequestHeader(requestHeader);
        }
        catch (Exception exp)
        {
            //ignored
        }
        return null;
    }

    /// <summary>
    /// Checking if tracing is registered
    /// </summary>
    /// <returns></returns>
    private static HttpDiagnosticSource? IfEnabled()
    {
        return DiagnosticContext.GetEnabled()
            ? new HttpDiagnosticSource(DiagnosticContext.MTRTracingActivitySource.Instance)
            : null;
    }

}
