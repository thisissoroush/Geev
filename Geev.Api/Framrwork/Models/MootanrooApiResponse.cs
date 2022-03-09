
namespace Geev.Api.Framrwork.Models;

/// <summary>
/// This class is used to create standard responses for AJAX/remote requests.
/// </summary>
[Serializable]
public class GeevApiResponse : GeevApiResponse<object>
{
    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object.
    /// <see cref="GeevApiResponseBase.Success"/> is set as true.
    /// </summary>
    public GeevApiResponse()
    {
    }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object with <see cref="GeevApiResponseBase.Success"/> specified.
    /// </summary>
    /// <param name="success">Indicates success status of the result</param>
    public GeevApiResponse(bool success)
        : base(success)
    {
    }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object with <see cref="GeevApiResponse{TResult}.Result"/> specified.
    /// <see cref="GeevApiResponseBase.Success"/> is set as true.
    /// </summary>
    /// <param name="result">The actual result object</param>
    public GeevApiResponse(object result)
        : base(result)
    {
    }

    /// <summary>
    /// Creates an <see cref="GeevApiResponse"/> object with <see cref="GeevApiResponseBase.Error"/> specified.
    /// <see cref="GeevApiResponseBase.Success"/> is set as false.
    /// </summary>
    /// <param name="error">Error details</param>
    /// <param name="unauthorizedRequest">Used to indicate that the current user has no privilege to perform this request</param>
    public GeevApiResponse(ErrorInfo error, bool unauthorizedRequest = false)
        : base(error, unauthorizedRequest)
    {
    }
}
