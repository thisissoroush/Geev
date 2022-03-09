using System.Net;

namespace Geev.Core.Exceptions;

/// <summary>
/// The request could not be completed due to a conflict with the current state of the target resource.
/// This code is used in situations where the user might be able to resolve the conflict and resubmit the request.
/// </summary>
public class GeevConflictException : GeevException, IHasHttpStatusCode
{
    public const int ExceptionCode = 7;

    public GeevConflictException(string message, string technicalMessage = "") : base(message, technicalMessage, ExceptionCode)
    {
    }

    public GeevConflictException(string message, string technicalMessage, Exception innerException) : base(message, technicalMessage, innerException, ExceptionCode)
    {
    }

    public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.Conflict;
}
