using FluentValidation;
using MediatR;
using Geev.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Geev.Services.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));


            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();


            IList<ValidationResult> validationErrors = new List<ValidationResult>();

            foreach (var failure in failures)
            {
                var validationError = new ValidationResult(failure.ErrorMessage, new[] { failure.PropertyName });
                validationErrors.Add(validationError);
            }


            if (validationErrors.Any())
                throw new GeevValidationException("داده‌های ورودی معتبر نیست. لطفا جزئیات را ببینید", validationErrors);

        }
        return await next();
    }
}
