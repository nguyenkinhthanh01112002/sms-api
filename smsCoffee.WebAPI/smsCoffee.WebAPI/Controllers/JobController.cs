using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.Interfaces;
using System.Data;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IBackgroundJobService _jobService;
        private readonly CoffeeDbContext _context;
        public JobController(IBackgroundJobService jobService, CoffeeDbContext context)
        {
            _jobService = jobService;
            _context = context;
        }
        [HttpPost]
        [Route("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Background Job Triggered"));
            return Ok();
        }
        [HttpPost]
        [Route("CreateSheduledJob")]
        public ActionResult CreateSheduledJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            BackgroundJob.Schedule(() => Console.WriteLine("Sheduled Job Triggered"), dateTimeOffset);
            return Ok();
        }

    }
}
