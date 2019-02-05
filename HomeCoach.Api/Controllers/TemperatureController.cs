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
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        [HttpPost("temperature")]
        public async Task<IActionResult> GetTemperature(SkillRequest skillRequest)
        {
            var requestType = skillRequest.GetRequestType();
            var netAtmoAccessToken = skillRequest.Context.System.User.AccessToken;

            if (String.IsNullOrEmpty(netAtmoAccessToken))
            {
                return Ok(ResponseBuilder.TellWithLinkAccountCard("Veuillez vous identifier à netatmo afin d'utiliser cette skill"));
            }

            IClient client = new Client(
                NodaTime.SystemClock.Instance, " https://api.netatmo.com/",
                String.Empty, String.Empty);
            client.ProvideOAuth2Token(netAtmoAccessToken);
            var netAtmoResult = await client.Air.GetHomeCoachsData();
            var device = netAtmoResult.Body.Devices.First();

            SkillResponse response =
                ResponseBuilder.Tell(
                    $"La température de {device.Name} est de {device.DashboardData.Temperature}°C et l'humidité est de {device.DashboardData.HumidityPercent}%");


            return this.Ok(response);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return this.Ok("API is alive");
        }
    }
}