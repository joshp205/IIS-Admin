using IISAdministration.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IISAdministration.Hubs {

    public class PollHub : Hub {

        public static readonly string GROUP_MAIN = "GroupMain";

        public void Connect(string server) {
            Server s = JsonConvert.DeserializeObject<Server>(server);

            Groups.AddToGroupAsync(Context.ConnectionId, s.Name);
            HubEventEmitter.ClientConnect(Context.ConnectionId, s.Name);
        }

        public void ConnectAggregate() {
            Groups.AddToGroupAsync(Context.ConnectionId, GROUP_MAIN);
            HubEventEmitter.ClientConnectAggregate(Context.ConnectionId);
        }

        public void Disconnect() {
            Server s = HubEventEmitter.GetServer(Context.ConnectionId);

            Groups.RemoveFromGroupAsync(Context.ConnectionId, s.Name);
            HubEventEmitter.ClientDisconnect(Context.ConnectionId);
        }

        public void DisconnectAggregate() {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, GROUP_MAIN);
            HubEventEmitter.ClientDisconnectAggregate(Context.ConnectionId);
        }
    }
}
