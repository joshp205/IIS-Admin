using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IISAdministration.Helpers {

    public class Rest {

        /// <summary>
        /// Sends a GET request to the specified URL and returns the resulting message.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns></returns>
        public static async Task<string> Get(string url, string token) {
            string msg = string.Empty;
            using (HttpClient client = CreateClient(token)) {
                var stringTask = client.GetStreamAsync(url);
                var streamResult = await stringTask;
                using (StreamReader reader = new StreamReader(streamResult)) {
                    msg = reader.ReadToEnd();
                }
            }

            return msg;
        }

        private static HttpClient CreateClient(string token) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "IIS Admin Agent");
            client.DefaultRequestHeaders.Add("Access-Token", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
