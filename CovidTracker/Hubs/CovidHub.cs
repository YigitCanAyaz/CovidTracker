using CovidTracker.Models;
using Microsoft.AspNetCore.SignalR;

namespace CovidTracker.Hubs
{
    public class CovidHub : Hub
    {
        private readonly CovidService _covidService;

        public CovidHub(CovidService covidService)
        {
            _covidService = covidService;
        }

        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", _covidService.GetCovidTrackerList());
        }
    }
}
