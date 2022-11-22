using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Autobarn.Website.Hubs {
    public class AutobarnHub : Hub {
        public async Task NotifyAllThePeopleWhoAreOnOurWebsite(string user, string message) {
            await Clients.All.SendAsync("ShowAPopupNotificationAboutANewVehicle", user, message);
        }
    }
}
