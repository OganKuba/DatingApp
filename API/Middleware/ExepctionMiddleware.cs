

using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExepctionMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger<ExepctionMiddleware> _logger;
        readonly IHostEnvironment _env;
        public ExepctionMiddleware(RequestDelegate next , ILogger<ExepctionMiddleware> logger, IHostEnvironment env){
            _env = env;
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }
            catch(Exception ex){
                _logger.LogError(ex , ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()
                    ? new ApiExecption(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiExecption(context.Response.StatusCode, ex.Message, "Internal Server Error");
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}