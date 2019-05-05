using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotMan.Models;
using SpotMan.OptionModels;

namespace SpotMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : SpotManControllerBase
    {
        public SpotifyController(SpotifyOptions spotify)
        {
            Spotify = spotify;

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(Spotify.BaseUrl),
                Timeout = TimeSpan.FromSeconds(Spotify.TimeoutSeconds)
            };
        }

        private SpotifyOptions Spotify { get; }
        private HttpClient HttpClient { get; }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public ActionResult<User> GetCurrentUser()
        {
            return Success();
        }
    }
}