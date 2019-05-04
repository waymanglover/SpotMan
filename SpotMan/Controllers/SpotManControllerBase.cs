using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SpotMan.Controllers
{
    public abstract class SpotManControllerBase : ControllerBase
    {
        public static ActionResult Result(HttpStatusCode statusCode, string reason = null) =>
            new ContentResult
            {
                StatusCode = (int)statusCode,
                Content = $"Status Code: {(int)statusCode}; {statusCode}; {reason ?? string.Empty}",
                ContentType = "text/plain"
            };

        public static ActionResult Result(HttpStatusCode statusCode, Exception e) =>
            new ContentResult
            {
                StatusCode = (int)statusCode,
                Content = $"Status Code: {(int)statusCode}; {statusCode}; {e}",
                ContentType = "text/plain"
            };

        public static ActionResult Result(Exception e)
        {
            return Result(HttpStatusCode.InternalServerError, e);
        }
    }
}
