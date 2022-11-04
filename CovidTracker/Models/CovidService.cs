using CovidTracker.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CovidTracker.Models
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            // inform hub when it's added
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", "data");
        }
    }
}
