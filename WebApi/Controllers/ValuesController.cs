using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Configs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly HostingConfiguration _options;
        //private readonly HostingConfiguration _optionsAccessor;
        private readonly HostingConfiguration _snapshotOptions;
        public ValuesController(
            ILogger<ValuesController> logger,
            IOptions<HostingConfiguration> options,
            //IOptionsMonitor<HostingConfiguration> optionsAccessor,
            IOptionsSnapshot<HostingConfiguration> snapshotOptionsAccessor)
        {
            _logger = logger;
            _options = options.Value;
            //_optionsAccessor = optionsAccessor.CurrentValue;
            _snapshotOptions = snapshotOptionsAccessor.Value;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<object>> Get()
        {
           Thread.Sleep(_snapshotOptions.TimeSleep);
            if (_snapshotOptions.Error404)
            {
                _logger.LogInformation($"La peticion lanzo el error {0}", nameof(_snapshotOptions.Error404));
                return NotFound();
            }
            if (_snapshotOptions.Error400)
            {
                _logger.LogInformation($"La peticion lanzo el error {0}", nameof(_snapshotOptions.Error400));
                return BadRequest();
            }
            if (_snapshotOptions.Error401)
            {
                _logger.LogInformation($"La peticion lanzo el error {0}", nameof(_snapshotOptions.Error401));
                return Unauthorized();
            }
            if (_snapshotOptions.Error403)
            {
                _logger.LogInformation($"La peticion lanzo el error {0}", nameof(_snapshotOptions.Error403));
                return Forbid();
            }
            if (_snapshotOptions.Error500)
            {
                _logger.LogInformation($"La peticion lanzo el error {0}", nameof(_snapshotOptions.Error500));
                return new StatusCodeResult(500);
            }
            _logger.LogInformation($"La peticion lanzo el error {0}", 200);
            return new object[] { _options, _snapshotOptions };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
