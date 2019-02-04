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
            var netAtmoAccessToken = skillRequest.Context.System.User.AccessToken;

            if (String.IsNullOrEmpty(netAtmoAccessToken))
            {
                return Ok(ResponseBuilder.TellWithLinkAccountCard("Veuillez vous identifier à netatmo afin d'utiliser cette skill"));
            }

            SkillResponse response = ResponseBuilder.Tell($"Welcome to HomeCoach! requestType {requestType}");

       
            return this.Ok(response);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return this.Ok("API is alive");
        }
    }
}