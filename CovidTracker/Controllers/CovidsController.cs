using CovidTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);

            IQueryable<Covid> covids = _covidService.GetList();

            return Ok(covids);
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new Random();

            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid { City = item, Count = random.Next(100, 1000), CovidDate = DateTime.Now.AddDays(x) };
                    _covidService.SaveCovid(newCovid).Wait(); // wait until next iteration
                    System.Threading.Thread.Sleep(1000);
                }



            });

            return Ok("Covid data saved to the database");
        }
    }
}
