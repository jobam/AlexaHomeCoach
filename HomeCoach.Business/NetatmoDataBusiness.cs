namespace HomeCoach.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Netatmo;

    public class NetatmoDataBusiness : INetatmoDataBusiness
    {
        public async Task<IEnumerable<HomeCoachData>> GetDevicesData(string oauth2Token)
        {
            IClient client = new Client(
                NodaTime.SystemClock.Instance, " https://api.netatmo.com/",
                String.Empty, String.Empty);
            client.ProvideOAuth2Token(oauth2Token);
            var netAtmoResult = await client.Air.GetHomeCoachsData();
            
            return new List<HomeCoachData>();
        }
    }
}