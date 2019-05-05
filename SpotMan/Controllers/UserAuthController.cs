using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotMan.Extensions;
using SpotMan.Helpers;
using SpotMan.OptionModels;

namespace SpotMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : SpotManControllerBase
    {
        public UserAuthController(UserAuthOptions userAuth)
        {
            UserAuth = userAuth;
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(UserAuth.BaseUrl),
                Timeout = TimeSpan.FromSeconds(UserAuth.TimeoutSeconds)
            };
        }

        private HttpClient HttpClient { get; }
        private UserAuthOptions UserAuth { get; }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public ActionResult AuthorizeUser()
        {
            try
            {
                var urlParams = new Dictionary<string, string>
                {
                    {"client_id", UserAuth.ClientId},
                    {"response_type", "code"},
                    {"redirect_uri", UserAuth.BaseUrl + Constants.LocalUrlAuthCallback},
                    {"scope", string.Join(' ', UserAuth.Scopes)}
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    return Result(StatusCodes.Status401Unauthorized, $"Error: {error}, State: {state ?? "NULL"}");

                if (string.IsNullOrWhiteSpace(code))
                    return Result(StatusCodes.Status400BadRequest, "Error: Empty or null code returned, " +
                                                                   $"State: {state ?? "NULL"}");

                var tokenRequest = new AuthorizationCodeTokenRequest
                {
                    ClientId = UserAuth.ClientId,
                    ClientSecret = UserAuth.ClientSecret,
                    Code = code,
                    Address = Constants.UrlSpotifyToken,
                    RedirectUri = UserAuth.BaseUrl + Constants.LocalUrlAuthCallback
                };
                var tokenRequestTime = DateTimeOffset.UtcNow;
                var tokenResponse = await HttpClient.RequestAuthorizationCodeTokenAsync(tokenRequest);

                if (tokenResponse.IsError)
                    throw new ApplicationException("Failed to get token authorization code token."
                                                   + Environment.NewLine
                                                   + $"{tokenResponse.ErrorType} Error:"
                                                   + $"{tokenResponse.Error} {tokenResponse.ErrorDescription}");
                // ReSharper disable once InconsistentNaming
                var Expiry = tokenRequestTime + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
                var keysToStore = new Dictionary<string, string>
                {
                    {nameof(tokenResponse.AccessToken), tokenResponse.AccessToken},
                    {nameof(tokenResponse.RefreshToken), tokenResponse.RefreshToken},
                    {nameof(Expiry), Expiry.ToString(CultureInfo.InvariantCulture)}
                };
                PersistenceHelper.StoreKeys(keysToStore);

                return Success();
            }
            catch (Exception ex)
            {
                return Result(ex);
            }
        }
    }
}