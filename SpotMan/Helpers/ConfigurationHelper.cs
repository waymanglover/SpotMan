using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SpotMan.Helpers
{
    public class ConfigurationHelper
    {
        public ConfigurationHelper(IConfiguration configuration)
        {
            UserAuth = configuration.GetSection(nameof(UserAuth));
            Scopes = UserAuth.GetSection(nameof(Scopes))
                .AsEnumerable()
                .Select(kvp => kvp.Value);
        }

        // Values that shouldn't change often
        public IConfigurationSection UserAuth { get; }
        public string ClientId => UserAuth[nameof(ClientId)];
        public string ClientSecret => UserAuth[nameof(ClientSecret)];
        public string SelfUrl => UserAuth[nameof(SelfUrl)];
        public int TimeoutSeconds => int.Parse(UserAuth[nameof(TimeoutSeconds)]);
        public IEnumerable<string> Scopes { get; }
    }
}