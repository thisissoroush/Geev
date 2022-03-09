using System.Net;

namespace Geev.Core.Exceptions;

/// <summary>
/// Every exception that implements this exception, can exactly describe it's own HttpStatusCode.
/// The framework will use the provider http-code instead of it's default codes.
/// </summary>
public interface IHasHttpStatusCode
{
    HttpStatusCode HttpStatusCode { get; }
}
