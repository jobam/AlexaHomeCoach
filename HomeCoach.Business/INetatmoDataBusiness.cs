namespace HomeCoach.Business
{
    using System.Collections.Generic;
    using Models;

    public interface INetatmoDataBusiness
    {
        IEnumerable<HomeCoachData> GetDevicesData(string oauth2Token);
    }
}