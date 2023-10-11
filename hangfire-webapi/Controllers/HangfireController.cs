using Hangfire;
using hangfire_webapi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hangfire_webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HangfireController : ControllerBase
    {
        private readonly ILogger<HangfireController> _logger;

        public HangfireController(ILogger<HangfireController> logger)
        {
            _logger = logger;
        }

        // Fire-and-forget job
        [HttpPost]
        [Route("[Action]")]
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome to our app"));

            return Ok($"Job Id: {jobId}");
        }

        // Delayed job
        [HttpPost]
        [Route("[Action]")]
        public IActionResult Discount()
        {
            int time = 30;

            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Welcome to our app"), TimeSpan.FromSeconds(time));

            return Ok($"Job Id: {jobId}. Discount email will be sent in {time} seconds.");
        }

        // Recurring job
        [HttpPost]
        [Route("[Action]")]
        public IActionResult DatebaseUpdate()
        {
            RecurringJob.AddOrUpdate("database-update-job", () => Console.WriteLine("Database updated"), Cron.Minutely);

            return Ok("Database check job initiated.");
        }

        //Continuous job
        [HttpPost]
        [Route("[Action]")]
        public IActionResult Confirm()
        {
            int time = 30;

            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be unsubscribed!"), TimeSpan.FromSeconds(time));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed."));

            return Ok($"Confirmation job created.");
        }



    }
}