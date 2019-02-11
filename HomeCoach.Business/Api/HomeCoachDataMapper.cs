namespace HomeCoach.Business
{
    using System.Collections.Generic;
    using Models;
    using Netatmo.Models.Client;
    using Netatmo.Models.Client.Air;
    using Netatmo.Models.Client.Air.HomesCoachs;

    public class HomeCoachDataMapper : IHomeCoachDataMapper
    {
        public HomeCoachData Map(Devices source)
        {
            if (source.DashboardData == null)
            {
                throw new NoDataException();
            }
            
            var dest = new HomeCoachData()
            {
                Temperature = source.DashboardData.Temperature,
                DeviceName = source.Name,
                HumidityPercent = source.DashboardData.HumidityPercent,
                Noise = source.DashboardData.Noise,
                Co2 = source.DashboardData.CO2
            };

            return dest;
        }

        public IEnumerable<HomeCoachData> Map(DataResponse<GetHomeCoachsData> source)
        {
            var dest = new List<HomeCoachData>();

            foreach (var device in source.Body.Devices)
            {
                dest.Add(this.Map(device));
            }
            
            return dest;
        }
    }

    public interface IHomeCoachDataMapper
    {
        HomeCoachData Map(Devices source);
        IEnumerable<HomeCoachData> Map(DataResponse<GetHomeCoachsData> source);
    }
}