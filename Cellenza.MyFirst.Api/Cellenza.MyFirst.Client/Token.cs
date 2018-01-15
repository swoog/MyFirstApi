using Newtonsoft.Json;

namespace Cellenza.MyFirst.Client
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}