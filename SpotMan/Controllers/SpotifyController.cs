using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpotMan.Helpers;
using SpotMan.Models;

namespace SpotMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : SpotManControllerBase
    {
        private HttpClient HttpClient { get; }

        public SpotifyController(IConfiguration configuration)
        {
            HttpClient = new HttpClient()
            {
                BaseAddress = new Uri(ConfigurationHelper.SelfUrl),
                Timeout = TimeSpan.FromSeconds(ConfigurationHelper.TimeoutSeconds),
            };
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public ActionResult<User> GetCurrentUser()
        {
            return Success();
        }
    }
}
