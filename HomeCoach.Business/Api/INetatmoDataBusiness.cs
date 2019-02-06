namespace HomeCoach.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface INetatmoDataBusiness
    {
        Task<IEnumerable<HomeCoachData>> GetDevicesData(string oauth2Token);
    }
}