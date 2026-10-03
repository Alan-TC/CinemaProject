using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Communication.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace CinemaBackend.Admin.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is CinemaBackendExceptionBase cinemaException)
        {
            var response = new ErrorResponseJson(cinemaException.GetErrorMessages());

            httpContext.Response.StatusCode = (int)cinemaException.GetErrorCode();

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }
        else
        {
            var response = new ErrorResponseJson(ResourceErrorMessages.UNKNOWN_ERROR);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }

        return true;
    }
}
