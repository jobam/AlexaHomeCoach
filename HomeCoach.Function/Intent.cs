using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using HomeCoach.Business;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace HomeCoach.Function
{

    public static class Intent
    {
        private const string responseString =
          "La température de {0} est de {1}°C, l'humidité est de {2}%, le niveau de CO2 est de {3} PPM et le bruit est de {4} décibels";

        [FunctionName("IntentFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            SkillRequest skillRequest = await req.Content.ReadAsAsync<SkillRequest>();

            var mapper = new HomeCoachDataMapper();
            var business = new NetatmoDataBusiness(mapper);

            var netAtmoAccessToken = skillRequest.Context.System.User.AccessToken;
            return req.CreateResponse(HttpStatusCode.OK, ResponseBuilder.Tell($"Token: {netAtmoAccessToken}"));

            //if (String.IsNullOrEmpty(netAtmoAccessToken))
            //{
            //    return req.CreateResponse(HttpStatusCode.OK, ResponseBuilder.TellWithLinkAccountCard("Veuillez vous identifier à netatmo afin d'utiliser cette skill"));
            //}

            //try
            //{
            //    var devicesData = await business.GetDevicesData(netAtmoAccessToken);
            //    var device = devicesData.First();

            //    SkillResponse response =
            //        ResponseBuilder.Tell(String.Format(responseString,
            //            device.DeviceName,
            //            device.Temperature.ToString().Replace(".", ","),
            //            device.HumidityPercent,
            //            device.Co2,
            //            device.Noise)
            //        );


            //    return req.CreateResponse(HttpStatusCode.OK, response);
            //}
            //catch (NoDataException)
            //{
            //    return req.CreateResponse(HttpStatusCode.OK, ResponseBuilder.Tell("Impossible de récupérer les données depuis Netatmo"));
            //}
        }
    }
}
