using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netatmo;
using NodaTime;

namespace HomeCoach.Api.Controllers
{
    using Alexa.NET;
    using Alexa.NET.Request;
    using Alexa.NET.Request.Type;
    using Alexa.NET.Response;
    using Business;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    [ApiController]
    public class IntentController : ControllerBase
    {
        private readonly INetatmoDataBusiness business;

        public IntentController(INetatmoDataBusiness business)
        {
            this.business = business;
        }
        
        [HttpPost("devices")]
        public async Task<IActionResult> GetDevicesData(SkillRequest skillRequest)
        {
            var requestType = skillRequest.GetRequestType();
            var netAtmoAccessToken = skillRequest.Context.System.User.AccessToken;

            if (String.IsNullOrEmpty(netAtmoAccessToken))
            {
                return Ok(ResponseBuilder.TellWithLinkAccountCard("Veuillez vous identifier à netatmo afin d'utiliser cette skill"));
            }

            var devicesData = await this.business.GetDevicesData(netAtmoAccessToken);

            var device = devicesData.First();

            SkillResponse response =
                ResponseBuilder.Tell(
                    $"La température de {device.DeviceName} est de {device.Temperature}°C et l'humidité est de {device.HumidityPercent}%");


            return this.Ok(response);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return this.Ok("API is alive");
        }
    }
}