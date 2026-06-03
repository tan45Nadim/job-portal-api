using System.Text.Json;
using JobPortalAPI.API.DTOs.Common;
using JobPortalAPI.API.Exceptions;

namespace JobPortalAPI.API.Middleware;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;

  public ExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
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
