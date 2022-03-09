namespace Geev.Core.Exceptions;

public class GeevInvalidOperationException : GeevException
{
    public const int ExceptionCode = 5;

    public GeevInvalidOperationException(string message, string technicalMessage = "", string operation = "") : base(message, technicalMessage, ExceptionCode)
    {
        Operation = operation;
    }

    public GeevInvalidOperationException(string message, string technicalMessage, string operation, Exception innerException) : base(message, technicalMessage, innerException, ExceptionCode)
    {
        Operation = operation;
    }

    public string Operation { get; }
}
