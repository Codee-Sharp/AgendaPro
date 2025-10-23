using System;
using AgendaPro.Api.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgendaPro.Api.Filters;

public class ApiResponseValidationFilter : IResultFilter
{
    private readonly IHostEnvironment _env;
    private readonly ILogger<ApiResponseValidationFilter> _logger;

    public ApiResponseValidationFilter(IHostEnvironment env, ILogger<ApiResponseValidationFilter> logger)
    {
        _env = env;
        _logger = logger;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        var result = context.Result;

        if (result is ObjectResult objectResult)
        {
            var value = objectResult.Value;

            if (value is ProblemDetails || value is ValidationProblemDetails ||
             (value != null && value.GetType()
             .IsGenericType && value.GetType().
             GetGenericTypeDefinition() == typeof(ApiResponse<>)))
            {
                return;
            }

            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];
            var methodFullName = $"{controllerName}Controller.{actionName}";

            if (_env.IsDevelopment() || _env.IsEnvironment("Testing"))
            {
                throw new InvalidCastException(
                                $"O retorno do método {methodFullName} não está no padrão ApiResponse. " +
                                $"Tipo retornado: {value?.GetType().Name ?? "null"}");
            }

            _logger.LogWarning(
                        "Resposta fora do padrão no método {Method}. Encapsulando automaticamente no formato ApiResponse.",
                        methodFullName
                        );

            var wrapped = Activator.CreateInstance(typeof(ApiResponse<>)
            .MakeGenericType(value?.GetType() ?? typeof(object)),
            value ?? new object());

            context.Result = new ObjectResult(wrapped)
            {
                StatusCode = objectResult.StatusCode ?? 200
            };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        
    }
}
