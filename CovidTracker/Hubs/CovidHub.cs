using Microsoft.AspNetCore.SignalR;

namespace CovidTracker.Hubs
{
    public class CovidHub : Hub
    {
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", "take covid data from service");
        }
    }
}
