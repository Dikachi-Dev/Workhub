using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Workhub.Api.EndPoints
{
    public static class EndpointBase
    {

        public static ProblemDetails GetProblemDetails(List<Error> errors)
        {
            var firstError = errors.FirstOrDefault();
            var statusCode = firstError.Type switch
            {
                ErrorType.Conflict => (int?)StatusCodes.Status409Conflict,
                ErrorType.Validation => (int?)StatusCodes.Status400BadRequest,
                ErrorType.NotFound => (int?)StatusCodes.Status404NotFound,
                _ => (int?)StatusCodes.Status500InternalServerError
            };

            var title = firstError.Description;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode ?? StatusCodes.Status500InternalServerError,
                Title = title ?? "Internal Server Error"
            };

            return problemDetails;
        }
    }
}