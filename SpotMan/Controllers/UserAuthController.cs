using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.Extensions.Configuration;
using SpotMan.Helpers;

namespace SpotMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : SpotManControllerBase
    {
        private readonly ConfigurationHelper _configHelper;

        public UserAuthController(IConfiguration configuration)
        {
            _configHelper = new ConfigurationHelper(configuration);
        }

        // GET api/values
        [HttpGet]
        public ActionResult AuthorizeUser()
        {
            try
            {
                var urlParams = new Dictionary<string, string>
                {
                    {"client_id", _configHelper.ClientId},
                    {"response_type", "code"},
                    {"redirect_uri", _configHelper.RedirectUri},
                    {"scope" , string.Join(' ', _configHelper.Scopes)}
                    // TODO: Add scope/state
                };
                var urlParamString = '?' +
                                     string.Join('&',
                                         urlParams.Select(p =>
                                             HttpUtility.UrlEncode(p.Key) + '=' + HttpUtility.UrlEncode(p.Value)
                                         ));
                return Redirect(Constants.UrlSpotifyAccountAuthorize + urlParamString);
            }
            catch (Exception ex)
            {
                return Result(ex);
            }
        }

        [HttpGet("callback")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult AuthorizeUserCallback(string code,
            string error,
            string state)
        {
            try
            {
                if (!string.IsNullOrEmpty(error))
                {
                    return Result(HttpStatusCode.Unauthorized, $"Error: {error}, State: {state ?? "NULL"}");
                }

                if (string.IsNullOrWhiteSpace(code))
                {
                    return Result(HttpStatusCode.BadRequest, $"Error: Empty or null code returned, " +
                                                             $"State: {state ?? "NULL"}");
                }

                return Result(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Result(ex);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}