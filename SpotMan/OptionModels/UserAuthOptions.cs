using System.Collections.Generic;

namespace SpotMan.OptionModels
{
    public class UserAuthOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}