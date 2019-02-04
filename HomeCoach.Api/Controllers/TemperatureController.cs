using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeCoach.Api.Controllers
{
    using Alexa.NET;
    using Alexa.NET.Request;
    using Alexa.NET.Request.Type;
    using Alexa.NET.Response;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        [HttpPost("temperature")]
        public IActionResult GetTemperature(SkillRequest skillRequest)
        {
            var requestType = skillRequest.GetRequestType();

            SkillResponse response = ResponseBuilder.Tell("Welcome to HomeCoach!");

            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Welcome to HomeCoach!, voici une launch request !");
                response.Response.ShouldEndSession = false;
            }

            return this.Ok(response);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return this.Ok("API is alive");
        }
    }
}