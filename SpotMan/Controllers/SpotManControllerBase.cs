using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace SpotMan.Controllers
{
    public abstract class SpotManControllerBase : ControllerBase
    {
        public static ActionResult Result(int statusCode, string message = null)
        {
            return new ContentResult
            {
                StatusCode = statusCode,
                Content =
                    $"Status Code: {statusCode}; {ReasonPhrases.GetReasonPhrase(statusCode)}; {message ?? string.Empty}",
                ContentType = "text/plain"
            };
        }

        public static ActionResult Result(int statusCode, Exception e)
        {
            return Result(statusCode, e.ToString());
        }

        public static ActionResult Result(Exception e)
        {
            return Result(StatusCodes.Status500InternalServerError, e);
        }

        public static ActionResult Success(string message = null)
        {
            return Result(StatusCodes.Status200OK, message);
        }
    }
}