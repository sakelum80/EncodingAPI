using Encoding.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Encoding.Core
{
    public static class ErrorExtensions
    {
        public static ProblemDetails ToProblemDetails(this Error error)
        {
            return new ProblemDetails
            {
                Type = GetErrorType(error.Code),
                Title = "Invalid request",
                Status = GetStatusCode(error.Code),
                Detail = error.Message
            };
        }

        private static int GetStatusCode(string errorCode) =>
            errorCode switch
            {
                "ENC.400" => StatusCodes.Status400BadRequest,
                "ENC.500" => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest
            };

        private static string GetErrorType(string errorCode) =>
            $"https://errors.runlengthencoding.com/{errorCode}";
    }
}
