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
    using Business.Models;
    using Business.Response;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    [ApiController]
    public class IntentController : ControllerBase
    {
        private readonly INetatmoDataBusiness business;
        private readonly IResponseBusiness responseBusiness;

        public IntentController(INetatmoDataBusiness business, IResponseBusiness responseBusiness)
        {
            this.business = business;
            this.responseBusiness = responseBusiness;
        }

        [HttpPost("devices")]
        public async Task<IActionResult> GetDevicesData(SkillRequest skillRequest)
        {
            var intentRequest = skillRequest.Request as IntentRequest;

            var netAtmoAccessToken = skillRequest.Context.System.User.AccessToken;

            if (String.IsNullOrEmpty(netAtmoAccessToken))
            {
                return Ok(ResponseBuilder.TellWithLinkAccountCard("Veuillez vous identifier à netatmo afin d'utiliser cette skill"));
            }

            try
            {
                var devicesData = await this.business.GetDevicesData(netAtmoAccessToken);
                HomeCoachData requestedDeviceData;
                
                if (intentRequest.Intent.Slots.ContainsKey("device"))
                {
                    requestedDeviceData = devicesData.FirstOrDefault(x => x.DeviceName.ToLower() == intentRequest.Intent.Slots["device"].Value);
                    if (requestedDeviceData == null)
                    {
                        return Ok(ResponseBuilder.Tell($"Nous n'avons pas trouvé d'appareil {intentRequest.Intent.Slots["device"].Value}"));
                    }
                }
                else
                {
                    requestedDeviceData = devicesData.First();
                }
      
                SkillResponse response = ResponseBuilder.Tell(responseBusiness.BuildResponse(requestedDeviceData, intentRequest.Intent.Name));


                return this.Ok(response);
            }
            catch (NoDataException)
            {
                return Ok(ResponseBuilder.Tell("Impossible de récupérer les données depuis Netatmo"));
            }
        }
    }
}