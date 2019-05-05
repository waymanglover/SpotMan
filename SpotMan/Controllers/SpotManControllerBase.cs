using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;

namespace SpotMan.Controllers
{
    public abstract class SpotManControllerBase : ControllerBase
    {
        public static ActionResult Result(int statusCode, string message = null) =>
            new ContentResult
            {
                StatusCode = statusCode,
                Content = $"Status Code: {statusCode}; {ReasonPhrases.GetReasonPhrase(statusCode)}; {message ?? string.Empty}",
                ContentType = "text/plain"
            };

        public static ActionResult Result(int statusCode, Exception e) => 
            Result(statusCode, e.ToString());

        public static ActionResult Result(Exception e) => 
            Result(StatusCodes.Status500InternalServerError, e);

        public static ActionResult Success(string message = null) =>
            Result(StatusCodes.Status200OK, message);
    }
}
