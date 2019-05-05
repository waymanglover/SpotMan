using System;
using SpotMan.Helpers;

namespace SpotMan.Models
{
    public class OAuthToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset Expiry { get; set; }

        public OAuthToken()
        {
            var spotManRegistry = PersistenceHelper.GetKeys();
            AccessToken = spotManRegistry[nameof(AccessToken)];
            RefreshToken = spotManRegistry[nameof(RefreshToken)];
            Expiry = DateTimeOffset.Parse(spotManRegistry[nameof(Expiry)]);
        }
    }
}