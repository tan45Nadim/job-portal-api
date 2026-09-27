using System.Text.Json;
using JobPortalAPI.API.DTOs.Common;
using JobPortalAPI.API.Exceptions;

namespace JobPortalAPI.API.Middleware;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionMiddleware> _logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "An unhandled exception occurred while processing the request. Method: {Method}, Path: {Path}",
        httpContext.Request.Method,
        httpContext.Request.Path
      );

      await HandleExceptionAsync(httpContext, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    int statusCode;

    switch (exception)
    {
      case NotFoundException:
        statusCode = StatusCodes.Status404NotFound;
        break;

      case BadRequestException:
        statusCode = StatusCodes.Status400BadRequest;
        break;

      case ForbiddenException:
        statusCode = StatusCodes.Status403Forbidden;
        break;

      default:
        statusCode =
            StatusCodes.Status500InternalServerError;
        break;
    }

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = statusCode;

    var response = new ErrorResponseDto
    {
      StatusCode = statusCode,
      Message = exception.Message
    };

    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

  }


}
