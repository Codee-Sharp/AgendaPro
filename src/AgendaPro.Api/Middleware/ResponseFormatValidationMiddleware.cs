using System;
using System.Text.Json;

namespace AgendaPro.Api.Middleware
{
    public class ResponseFormatValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseFormatValidationMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ResponseFormatValidationMiddleware(RequestDelegate next, ILogger<ResponseFormatValidationMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(memoryStream).ReadToEndAsync();

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            if (string.IsNullOrWhiteSpace(bodyText)) return;

            try
            {
                var json = JsonDocument.Parse(bodyText);

                bool isValid =
                json.RootElement.TryGetProperty("data", out _) || json.RootElement.TryGetProperty("errors", out _);

                if (!isValid)
                {
                    if (_env.IsDevelopment())
                    {
                        throw new InvalidOperationException("Resposta fora do padrão");
                    }
                    else
                    {
                        _logger.LogWarning("Resposta fora do padrão foi detectada");

                        var wrapped = new
                        {
                            success = true,
                            data = JsonSerializer.Deserialize<object>(bodyText),
                            message = "Resposta fora do padrão encapsulada automaticamente"
                        };

                        context.Response.ContentType = "Application/json";
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(wrapped));
                    }
                }
            }
            catch (JsonException)
            {
                _logger.LogError("Falha ao validar o formado da resposta");
            }
        }
    }
}
