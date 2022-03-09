namespace Geev.Api.Framrwork.Models;

/// <inheritdoc/>
internal class ErrorInfoBuilder : IErrorInfoBuilder
{
    private readonly ILogger<ErrorInfoBuilder>? _logger;
    private IExceptionToErrorInfoConverter Converter { get; set; }

    // TODO: Logger injection does not work
    /// <inheritdoc/>
    public ErrorInfoBuilder(IWebHostEnvironment env)
    {
        _logger = null;
        AddExceptionConverter(new DefaultErrorInfoConverter(env, null));

    }

    /// <inheritdoc/>
    public ErrorInfo BuildForException(Exception exception, string source)
    {
        var errorInfo = Converter.Convert(exception);
        if (string.IsNullOrEmpty(errorInfo.Source))
            errorInfo.Source = source;
        return errorInfo;
    }

    public Exception BuildFromErrorInfo(ErrorInfo errorInfo)
    {
        return Converter.ReverseConvert(errorInfo);
    }

    public void AddExceptionConverter(IExceptionToErrorInfoConverter converter)
    {
        if (converter != null)
            converter.Next = Converter;
        Converter = converter;
    }
}
