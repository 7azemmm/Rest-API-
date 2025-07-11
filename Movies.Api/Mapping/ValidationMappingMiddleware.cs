using FluentValidation;
using FluentValidation.Results;
using Movies.Contracts.Responses;

namespace RestApi.Mapping;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMappingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            ValidationFailureResponse validationFailure = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x=> new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };
            context.Response.WriteAsJsonAsync(validationFailure);
        }
    }
}