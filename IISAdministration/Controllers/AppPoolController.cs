using IISAdministration.Models;
using IISAdministration.Models.DatabaseModels;
using IISAdministration.Models.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace IISAdministration.Controllers {

    [Authorize]
    public class AppPoolController : Controller {
        private readonly ServersContext _srvContext;
        
        public AppPoolController(ServersContext context) {
            _srvContext = context;
        }

        [HttpGet("/AppPool/{id}")]
        public IActionResult Index(int id) {
            ViewBag.ServerId = id;
            ViewBag.AppPools = GetAppPoolDetails(id);
            return View();
        }

        public IActionResult StartAppPool(string id, int srvId) {
            PatchApplicationPool(id, srvId, "{\"status\":\"started\"}");
            return Redirect($"/AppPool/{srvId}");
        }

        public IActionResult StopAppPool(string id, int srvId) {
            PatchApplicationPool(id, srvId, "{\"status\":\"stopped\"}");
            return Redirect($"/AppPool/{srvId}");
        }

        private void PatchApplicationPool(string id, int srvId, string body) {
            Server serv = GetServerInstance(srvId);

            var client = new RestClient(serv.Url);
            var request = new RestRequest($"api/webserver/application-pools/{id}", Method.PATCH);
            request.AddHeader("Access-Token", $"Bearer {serv.Token}");
            request.AddJsonBody(body);
            IRestResponse response = client.Execute(request);
        }

        private AppPoolJsonData GetAppPoolDetails(int id) {
            Server serv = GetServerInstance(id);

            var client = new RestClient(serv.Url);
            var request = new RestRequest("api/webserver/application-pools", Method.GET);
            request.AddHeader("Access-Token", $"Bearer {serv.Token}");

            IRestResponse response = client.Execute(request);
            var appPools = JsonConvert.DeserializeObject<AppPoolJsonData>(response.Content);

            return appPools;
        }

        private Server GetServerInstance(int id) {
            var result = _srvContext.Servers.SingleAsync(s => s.ServerId == id);
            result.Wait();
            return result.Result;
        }
    }
}