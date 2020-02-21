using Newtonsoft.Json;
using System.Collections.Generic;

namespace IISAdministration.Models.JsonModels {

    public class AppPoolJsonData {
        [JsonProperty("app_pools")]
        public List<AppPoolJson> Data { get; private set; }
    }

    public class AppPoolJson {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }
    }
}
