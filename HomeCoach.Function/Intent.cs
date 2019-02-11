using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace HomeCoach.Function
{
    public static class Intent
    {
        [FunctionName("IntentFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            SkillRequest data = await req.Content.ReadAsAsync<SkillRequest>();
            var response = ResponseBuilder.TellWithLinkAccountCard("Azure function is working !");

            return req.CreateResponse(HttpStatusCode.OK, response);
        }

        //[FunctionName("testFunction")]
        //public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        //{
        //    log.Info("C# HTTP trigger function processed a request.");

        //    return req.CreateResponse(HttpStatusCode.OK, "Function is alive !");
        //}
    }
}
