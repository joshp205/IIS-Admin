using IISAdministration.Models;
using IISAdministration.Models.DatabaseModels;
using IISAdministration.Models.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace IISAdministration.Controllers {

    /// <summary>
    /// Controller for the server pages of the application.
    /// </summary>
    [Authorize]
    public class ServersController : Controller {
        private readonly ServersContext _srvContext;

        public ServersController(ServersContext context) {
            _srvContext = context;
        }

        public IActionResult Index() {
            var servers = _srvContext.Servers.ToListAsync();
            servers.Wait();
            ViewBag.Servers = servers.Result;
            return View("Index");
        }

        [HttpGet]
        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name, string url, string token) {
            Server s = new Server(name, url, token);
            _srvContext.Servers.Add(s);
            _srvContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id) {
            var result = _srvContext.Servers.SingleAsync(s => s.ServerId == id);
            result.Wait();

            ViewBag.ServerId = id;
            return View(result.Result);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string url, string token) {
            var result = _srvContext.Servers.SingleOrDefaultAsync(s => s.ServerId == id);

            result.Result.Name = name;
            result.Result.Url = url;
            result.Result.Token = token;
            _srvContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id) {
            var result = _srvContext.Servers.SingleAsync(s => s.ServerId == id);
            result.Wait();
            _srvContext.Remove(result.Result);
            _srvContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/servers/{id}")]
        public IActionResult Server(int id) {
            var t = GetWebServerDetails(id);
            t.Wait();
            ViewBag.Server = t.Result.Item1;
            ViewBag.Websites = t.Result.Item2;

            return View();
        }

        private async Task<Tuple<Server, WebsiteJsonData>> GetWebServerDetails(int id) {
            var result = _srvContext.Servers.SingleAsync(s => s.ServerId == id);
            result.Wait();

            Server serv = result.Result;

            var client = new RestClient(serv.Url);
            var request = new RestRequest("/api/webserver/websites", Method.GET);
            request.AddHeader("Access-Token", $"Bearer {serv.Token}");

            IRestResponse response = client.Execute(request);
            var websites = JsonConvert.DeserializeObject<WebsiteJsonData>(response.Content);

            string msg;
            for (int i = 0; i < websites.Data.Count; i++) {
                request = new RestRequest($"/api/webserver/websites/{websites.Data[i].Id}", Method.GET);
                request.AddHeader("Access-Token", $"Bearer {serv.Token}");

                response = client.Execute(request);
                websites.Data[i] = JsonConvert.DeserializeObject<WebsiteJson>(response.Content);
            }

            return new Tuple<Server, WebsiteJsonData>(serv, websites);
        }
    }
}