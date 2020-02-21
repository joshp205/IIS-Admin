using Newtonsoft.Json;
using System.Collections.Generic;

namespace IISAdministration.Models.JsonModels {

    public class WebsiteJsonData {
        [JsonProperty("websites")]
        public List<WebsiteJson> Data { get; private set; }
    }

    public class WebsiteJson {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("physical_path")]
        public string PhysicalPath { get; private set; }

        [JsonProperty("key")]
        public int Key { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("server_auto_start")]
        public bool ServerAutoStart { get; private set; }

        [JsonProperty("enabled_protocols")]
        public string EnabledProtocols { get; private set; }

        [JsonProperty("limits")]
        public WebsiteLimitsJson Limits { get; private set; }

        [JsonProperty("application_pool")]
        public AppPoolJson AppPool { get; private set; }

        public bool Running {
            get { return Status.Equals("started"); }
        }
    }

    public class WebsiteLimitsJson {
        [JsonProperty("connection_timeout")]
        public double ConnectionTimeout { get; private set; }

        [JsonProperty("max_bandwidth")]
        public string MaxBandwidth { get; private set; }

        [JsonProperty("max_connections")]
        public string MaxConnection { get; private set; }

        [JsonProperty("max_url_segments")]
        public int MaxUrlSegments { get; private set; }
    }
}
