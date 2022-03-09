using Geev.Core.Exceptions;
using Geev.Core.Extensions;
using System.Text;

namespace Geev.Api.Framrwork.Models;

internal class DefaultErrorInfoConverter : IExceptionToErrorInfoConverter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger _logger;

    //private readonly IAbpWebCommonModuleConfiguration _configuration;
    //private readonly ILocalizationManager _localizationManager;

    public DefaultErrorInfoConverter(IWebHostEnvironment env, ILogger logger)
    {
        _env = env;
        _logger = logger;
    }

    public IExceptionToErrorInfoConverter Next { set; private get; }

    private bool IncludeErrorDetails
    {
        get
        {

            if (_env != null)
            {
                return !_env.IsProduction();
            }

            return true;
        }
    }

    public ErrorInfo Convert(Exception exception)
    {
        var errorInfo = CreateErrorInfoWithoutCode(exception);

        if (IncludeErrorDetails)
        {
            var detailBuilder = new StringBuilder();
            CreateDetailsFromException(exception, detailBuilder);
            errorInfo.Details = detailBuilder.ToString();
        }

        if (exception is GeevException mtrException)
        {
            errorInfo.ErrorCode = mtrException.ErrorCode ?? 0;
        }

        return errorInfo;
    }



    public Exception ReverseConvert(ErrorInfo errorInfo)
    {
        throw new NotImplementedException();
    }

    #region Private Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param message="exception"></param>
    /// <returns></returns>
    private ErrorInfo CreateErrorInfoWithoutCode(Exception exception)
    {
        if (exception is AggregateException aggException && exception.InnerException != null)
        {
            if (aggException.InnerException is GeevValidationException)
            {
                exception = aggException.InnerException;
            }
        }

        if (exception is GeevValidationException validationException)
        {
            return new ErrorInfo(L(validationException.Message))
            {
                ValidationErrors = GetValidationErrorInfoes(validationException),
                Details = GetValidationErrorNarrative(validationException)
            };
        }

        if (exception is EntityNotFoundException entityNotFoundException)
        {
            if (entityNotFoundException.EntityType != null)
            {
                return new ErrorInfo(
                    string.Format(
                        L("EntityNotFound"),
                        entityNotFoundException.EntityType.Name,
                        entityNotFoundException.Id
                    )
                );
            }

            return new ErrorInfo(L(entityNotFoundException.Message));
        }

        if (exception is AuthorizationException authorizationException)
        {
            return new ErrorInfo(L(authorizationException.Message));
        }

        if (exception is DuplicateRequestException duplicateRequestException)
        {
            return new ErrorInfo(L(duplicateRequestException.Message));
        }

        // Finally, check if it's a GeevException 
        if (exception is GeevException mtrException)
        {
            // Exlucde TechnicalMessage if it's Production env
            return new ErrorInfo(L(mtrException.Message), IncludeErrorDetails ? mtrException.TechnicalMessage : "");
        }

        return new ErrorInfo(L("InternalServerError"));
    }

    private void CreateDetailsFromException(Exception exception, StringBuilder detailBuilder)
    {
        //Exception Message
        detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

        //Additional info for UserFriendlyException
        if (exception is GeevException)
        {
            var userFriendlyException = exception as GeevException;
            if (!string.IsNullOrEmpty(userFriendlyException.TechnicalMessage))
            {
                detailBuilder.AppendLine(userFriendlyException.TechnicalMessage);
            }
        }

        //Additional info for AbpValidationException
        if (exception is GeevValidationException)
        {
            var validationException = exception as GeevValidationException;
            if (validationException.ValidationErrors.Count > 0)
            {
                detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
            }
        }

        //Exception StackTrace
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
        }

        //Inner exception
        if (exception.InnerException != null)
        {
            CreateDetailsFromException(exception.InnerException, detailBuilder);
        }

        //Inner exceptions for AggregateException
        if (exception is AggregateException aggException)
        {
            if (aggException.InnerExceptions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var innerException in aggException.InnerExceptions)
            {
                CreateDetailsFromException(innerException, detailBuilder);
            }
        }
    }

    private Dictionary<string, string> GetValidationErrorInfoes(GeevValidationException validationException)
    {
        var validationErrorInfos = new Dictionary<string, string>();

        int i = 1;
        foreach (var validationResult in validationException.ValidationErrors)
        {
            var validationError = new ValidationErrorInfo(validationResult.ErrorMessage);

            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                validationError.Members = validationResult.MemberNames.Select(m => m.ToCamelCase()).ToArray();
            }

            //If someone doesn't pass the member names correctly, we just use a counter instead
            var key = string.Join(",", validationError.Members) ?? (i++).ToString();
            if (!validationErrorInfos.ContainsKey(key))
            {
                validationErrorInfos.Add(key, validationError.Message);
            }
            else
            {
                var value = validationErrorInfos[key];
                validationErrorInfos[key] = value + ". " + validationError.Message;
            }
        }

        return validationErrorInfos;
    }

    private string GetValidationErrorNarrative(GeevValidationException validationException)
    {
        var detailBuilder = new StringBuilder();
        detailBuilder.AppendLine(L("ValidationNarrativeTitle"));

        foreach (var validationResult in validationException.ValidationErrors)
        {
            detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
            detailBuilder.AppendLine();
        }

        return detailBuilder.ToString();
    }

    private string L(string message)
    {
        //TODO: implement localization!
        try
        {
            if (message == "EntityNotFound")
                return "موجودیت مورد نظر یافت نشد";
            return message ?? "";
        }
        catch (Exception)
        {
            return message;
        }
    }

    #endregion

}
