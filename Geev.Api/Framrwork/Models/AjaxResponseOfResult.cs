namespace Geev.Api.Framrwork.Models;

/// <summary>
/// This class is used to create standard responses for AJAX requests.
/// </summary>
[Serializable]
public class GeevApiResponse<TResult> : GeevApiResponseBase
{
    /// <summary>
    /// The actual result object of AJAX request.
    /// It is set if <see cref="GeevApiResponseBase.Success"/> is true.
    /// </summary>
    public TResult Result { get; set; }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object with <see cref="Result"/> specified.
    /// <see cref="GeevApiResponseBase.Success"/> is set as true.
    /// </summary>
    /// <param name="result">The actual result object of AJAX request</param>
    public GeevApiResponse(TResult result)
    {
        Result = result;
        Success = true;
    }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object.
    /// <see cref="GeevApiResponseBase.Success"/> is set as true.
    /// </summary>
    public GeevApiResponse()
    {
        Success = true;
    }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object with <see cref="GeevApiResponseBase.Success"/> specified.
    /// </summary>
    /// <param name="success">Indicates success status of the result</param>
    public GeevApiResponse(bool success)
    {
        Success = success;
    }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object with <see cref="GeevApiResponseBase.Error"/> specified.
    /// <see cref="GeevApiResponseBase.Success"/> is set as false.
    /// </summary>
    /// <param name="error">Error details</param>
    /// <param name="unauthorizedRequest">Used to indicate that the current user has no privilege to perform this request</param>
    public GeevApiResponse(ErrorInfo error, bool unauthorizedRequest = false)
    {
        Error = error;
        UnauthorizedRequest = unauthorizedRequest;
        Success = false;
    }
}
