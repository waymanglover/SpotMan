using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotMan.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToUrlParamString(this Dictionary<string, string> urlParams)
        {
            return '?' + string.Join('&',
                       urlParams.Select(p =>
                           HttpUtility.UrlEncode(p.Key) + '=' + HttpUtility.UrlEncode(p.Value)
                       )
                   );
        }
    }
}