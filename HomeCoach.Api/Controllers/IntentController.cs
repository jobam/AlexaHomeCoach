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
        private readonly IIntentParsingBusiness intentParsingBusiness;

        public IntentController(INetatmoDataBusiness business, IResponseBusiness responseBusiness, IIntentParsingBusiness intentParsingBusiness)
        {
            this.business = business;
            this.responseBusiness = responseBusiness;
            this.intentParsingBusiness = intentParsingBusiness;
        }

        [HttpPost("devices")]
        public async Task<IActionResult> GetDevicesData(SkillRequest skillRequest)
        {
            var intentRequest = skillRequest.Request as IntentRequest;

            var netAtmoAccessToken = skillRequest?.Context?.System?.User?.AccessToken;

            if (String.IsNullOrEmpty(netAtmoAccessToken))
            {
                return Ok(ResponseBuilder.TellWithLinkAccountCard("Veuillez vous identifier à netatmo afin d'utiliser cette skill"));
            }

            try
            {
                var devicesData = await this.business.GetDevicesData(netAtmoAccessToken);
                var requestedDeviceData = this.intentParsingBusiness.GetDeviceData(devicesData, intentRequest.Intent?.Slots);

                SkillResponse response = ResponseBuilder.Tell(responseBusiness.BuildResponse(requestedDeviceData, intentRequest.Intent.Name));


                return this.Ok(response);
            }
            catch (NoDataException)
            {
                return Ok(ResponseBuilder.Tell("Impossible de récupérer les données depuis Netatmo"));
            }
            catch (DeviceNotFoundException ex)
            {
                return Ok(ResponseBuilder.Tell($"L'appareil {ex.DeviceName} n'a pas été trouvé"));
            }
        }
    }
}