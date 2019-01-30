using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HomeCoach.Api.Controllers
{
    using Alexa.NET;
    using Alexa.NET.Request;
    using Alexa.NET.Request.Type;
    using Alexa.NET.Response;

    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        [HttpPost]
        [Route("temperature")]
        public IActionResult GetTemperature(SkillRequest skillRequest)
        {          
            var requestType = skillRequest.GetRequestType();

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Welcome to HomeCoach!");
                response.Response.ShouldEndSession = false;
            }

            return this.Ok(response);
        }
        }

       
    }
}