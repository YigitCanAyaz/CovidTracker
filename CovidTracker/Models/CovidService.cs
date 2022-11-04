using CovidTracker.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidTrackerList());
        }

        public List<CovidTracker> GetCovidTrackerList()
        {
            List<CovidTracker> covidTrackers = new List<CovidTracker>();

            // Without entity framework select operation

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarih, [1], [2], [3], [4], [5] from\r\n(select[City], [Count], Cast([CovidDate] as date) as tarih FROM Covids) as covidT\r\nPIVOT\r\n(sum(Count) For City IN ([1], [2], [3], [4], [5])) as PTable\r\norder by tarih asc";

                command.CommandType = System.Data.CommandType.Text;

                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidTracker covidTracker = new CovidTracker();

                        covidTracker.CovidDate = reader.GetDateTime(0).ToShortDateString();

                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                covidTracker.Counts.Add(0);
                            }

                            else
                            {
                                covidTracker.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidTrackers.Add(covidTracker);
                    }
                }

                _context.Database.CloseConnection();

                return covidTrackers;
            }
        }
    }
}
