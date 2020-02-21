using Newtonsoft.Json;
using System.ComponentModel;

namespace IISAdministration.Models {

    public class Server {

        [JsonProperty("id")]
        public int ServerId { get; set; }

        [JsonProperty("name")]
        [DisplayName("Display Name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        [DisplayName("URL")]
        public string Url { get; set; }

        [JsonProperty("token")]
        [DisplayName("Token")]
        public string Token { get; set; }

        public Server(string name, string url, string token) {
            Name = name;
            Url = url;
            Token = token;
        }
    }
}
