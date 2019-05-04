using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SpotMan.Helpers
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ClientId => _configuration.GetSection("UserAuth")["ClientId"];
        public string RedirectUri => _configuration.GetSection("UserAuth")["RedirectUri"];
        public string RefreshToken => _configuration.GetSection("UserAuth")["RefreshToken"];
        public string Token => _configuration.GetSection("UserAuth")["Token"];
        public DateTime TokenExpiry
        {
            get => DateTime.Parse(_configuration.GetSection("UserAuth")["TokenExpiry"], CultureInfo.InvariantCulture);
            set => _configuration.GetSection("UserAuth")["TokenExpiry"] = value.ToString(CultureInfo.InvariantCulture);
        }

        public IEnumerable<string> Scopes => _configuration.GetSection("UserAuth")
            .GetSection("Scopes")
            .AsEnumerable()
            .Select(kvp => kvp.Value);
    }
}
