using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpotMan.Extensions;
using SpotMan.Helpers;

namespace SpotMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : SpotManControllerBase
    {
        private readonly ConfigurationHelper _configHelper;
        private readonly HttpClient _httpClient;

        public UserAuthController(IConfiguration configuration)
        {
            _configHelper = new ConfigurationHelper(configuration);
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_configHelper.SelfUrl),
                Timeout = TimeSpan.FromSeconds(_configHelper.TimeoutSeconds),
            };
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
                    {"redirect_uri", _configHelper.SelfUrl + Constants.LocalUrlAuthCallback},
                    {"scope", string.Join(' ', _configHelper.Scopes)}
                    // TODO: Add state
                };

                return Redirect(Constants.UrlSpotifyAuthorize + urlParams.ToUrlParamString());
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
        public async Task<ActionResult> AuthorizeUserCallback(string code,
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
                    return Result(HttpStatusCode.BadRequest, "Error: Empty or null code returned, " +
                                                             $"State: {state ?? "NULL"}");
                }

                var tokenRequest = new AuthorizationCodeTokenRequest()
                {
                    ClientId = _configHelper.ClientId,
                    ClientSecret = _configHelper.ClientSecret,
                    Code = code,
                    Address = Constants.UrlSpotifyToken,
                    RedirectUri = _configHelper.SelfUrl + Constants.LocalUrlAuthCallback,
                };
                var tokenResponse = await _httpClient.RequestAuthorizationCodeTokenAsync(tokenRequest);
                //_configHelper.Token = tokenResponse.AccessToken;

                return Result(HttpStatusCode.OK, tokenResponse.AccessToken);
            }
            catch (Exception ex)
            {
                return Result(ex);
            }
        }
    }
}