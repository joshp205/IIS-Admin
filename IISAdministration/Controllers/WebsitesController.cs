using IISAdministration.Models;
using IISAdministration.Models.DatabaseModels;
using IISAdministration.Models.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace IISAdministration.Controllers {

    /// <summary>
    /// Represents the controller for the individual website pages of the application.
    /// </summary>
    [Authorize]
    public class WebsitesController : Controller {
        private readonly ServersContext _srvContext;

        public WebsitesController(ServersContext context) {
            _srvContext = context;
        }

        public IActionResult Index() {

            return View();
        }

        [HttpGet("/servers/{serverId}/{id}")]
        public IActionResult Website(int serverId, string id) {
            var t = GetWebsiteDetails(serverId, id);
            t.Wait();
            ViewBag.Website = t.Result;
            return View();
        }

        private async Task<WebsiteJson> GetWebsiteDetails(int serverId, string id) {
            var servTask = _srvContext.Servers.SingleAsync(s => s.ServerId == serverId);
            servTask.Wait();

            Server server = servTask.Result;

            var client = new RestClient(server.Url);
            var request = new RestRequest($"/api/webserver/websites/{id}", Method.GET);
            request.AddHeader("Access-Token", $"Bearer {server.Token}");

            IRestResponse response = client.Execute(request);
            var website = JsonConvert.DeserializeObject<WebsiteJson>(response.Content);

            return website;
        }
    }
}