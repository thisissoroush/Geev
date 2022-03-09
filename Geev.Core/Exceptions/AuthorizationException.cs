namespace Geev.Core.Exceptions;

/// <summary>
/// Authorization Exception
/// TODO: Describe 401 and 403 usages (exception is used for both situatations)
/// </summary>
public class AuthorizationException : GeevException
{
    public const int ExceptionCode = 6;

    public AuthorizationException(string message, string technicalMessage = "") : base(message, technicalMessage, ExceptionCode)
    {
    }

    public AuthorizationException(string message, string technicalMessage, Exception innerException) : base(message, technicalMessage, innerException, ExceptionCode)
    {
    }
}
