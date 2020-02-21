using IISAdministration.Helpers;
using IISAdministration.Models;
using IISAdministration.Models.DatabaseModels;
using IISAdministration.Models.JsonModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace IISAdministration.Hubs {

    public class HubEventEmitter : BackgroundService {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<PollHub> _hubContext;

        private static readonly int _timeInterval = 3;
        private static bool _serverPolling = false;
        private static bool _aggregatePolling = false;
        private static bool _update = false;
        private static Timer _timer;

        private static List<Metrics> _metrics = new List<Metrics>();

        private static Dictionary<string, Server> _activeServers = new Dictionary<string, Server>();
        private static Dictionary<string, string> _clientIdsToServerNames = new Dictionary<string, string>();
        private static Dictionary<string, List<string>> _serverNamesToClientIds = new Dictionary<string, List<string>>();

        private static HashSet<string> _mainConnections = new HashSet<string>();

        public HubEventEmitter(IHubContext<PollHub> hubContext, IServiceScopeFactory scopeFactory) {
            // This needs to be removed in production {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            // }

            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
        }

        public static void ClientConnect(string id, string server) {
            _clientIdsToServerNames.Add(id, server);
            if (_serverNamesToClientIds.ContainsKey(server)) {
                _serverNamesToClientIds[server].Add(id);
            }
            else {
                List<string> l = new List<string>() { id };
                _serverNamesToClientIds.Add(server, l);
            }
            _serverPolling = true;
            StartTimer();
        }

        public static void ClientConnectAggregate(string id) {
            _mainConnections.Add(id);
            _aggregatePolling = true;
            StartTimer();
        }

        public static void ClientDisconnect(string id) {
            string server = _clientIdsToServerNames[id];
            _clientIdsToServerNames.Remove(id);
            _serverNamesToClientIds[server].Remove(id);
            if (_clientIdsToServerNames.Count <= 0) {
                _serverPolling = false;
            }
        }

        public static void ClientDisconnectAggregate(string id) {
            _mainConnections.Remove(id);
            if (_mainConnections.Count <= 0) {
                _aggregatePolling = false;
            }
        }

        public static Server GetServer(string client_id) {
            return _activeServers[_clientIdsToServerNames[client_id]];
        }

        public static void StartTimer() {
            if (_timer == null) {
                _timer = new Timer(Update, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(_timeInterval));
            }
        }

        public static void Update(object state) {
            _update = true;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                if (_update) {
                    bool aggregatePoll = _aggregatePolling;
                    bool serverPoll = _serverPolling;
                    if (aggregatePoll || serverPoll) {
                        using (var scope = _scopeFactory.CreateScope()) {
                            Metrics m;
                            MonitoringJson monitor;
                            var dbContext = scope.ServiceProvider.GetRequiredService<ServersContext>();

                            if (aggregatePoll) {
                                bool firstMetric = true;
                                Metrics total = new Metrics();
                                foreach (Server s in dbContext.Servers) {
                                    _activeServers.TryAdd(s.Name, s);

                                    try {
                                        monitor = RequestMetrics(s);
                                    }
                                    catch (Exception) {
                                        continue;
                                    }

                                    m = new Metrics();
                                    m.ServerId = s.ServerId;
                                    m.Timestamp = DateTime.UtcNow;
                                    m.Date = DateHelper.DateToInt(m.Timestamp);
                                    m.Time = DateHelper.TimeToInt(m.Timestamp);
                                    m.DayOfWeek = (int)m.Timestamp.DayOfWeek;
                                    monitor.CopyTo(ref m);

                                    if (firstMetric == true) {
                                        firstMetric = false;
                                        total = m;
                                    }
                                    else {
                                        total.Add(ref m);
                                    }

                                    if (serverPoll) {
                                        if (_serverNamesToClientIds[s.Name].Count > 0) {
                                            await _hubContext.Clients.Group(s.Name).SendAsync("ReceiveMessage", $"{JsonConvert.SerializeObject(monitor)}");
                                        }
                                    }
                                }

                                await _hubContext.Clients.Group(PollHub.GROUP_MAIN).SendAsync("ReceiveMessage", $"{JsonConvert.SerializeObject(total)}");
                            }
                            else {
                                foreach(string serverName in _serverNamesToClientIds.Keys) {
                                    if (_serverNamesToClientIds[serverName].Count > 0) {
                                        var server = dbContext.Servers.FirstOrDefault(s => s.Name == serverName);
                                        _activeServers.TryAdd(serverName, server);

                                        try {
                                            monitor = RequestMetrics(server);
                                        }
                                        catch (Exception) {
                                            continue;
                                        }

                                        await _hubContext.Clients.Group(serverName).SendAsync("ReceiveMessage", $"{JsonConvert.SerializeObject(monitor)}");
                                    }
                                }
                            }
                        }
                    }

                    _update = false;
                }
            }
        }

        private static Metrics GetMetrics() {

            return null;
        }

        private static MonitoringJson RequestMetrics(Server s) {
            var client = new RestClient(s.Url);
            client.Timeout = 3000;
            var request = new RestRequest("/api/webserver/monitoring/", Method.GET);
            request.AddHeader("Access-Token", $"Bearer {s.Token}");

            IRestResponse response = client.Execute(request);
            if (response.ErrorException != null) {
                throw response.ErrorException;
            }

            JObject data = JsonConvert.DeserializeObject<JObject>(response.Content);
            var monitor = MonitoringJson.ParseJson(data);

            return monitor;
        }
    }
}
