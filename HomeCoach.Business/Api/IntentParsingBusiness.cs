namespace HomeCoach.Business
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Alexa.NET;
    using Alexa.NET.Request;
    using Models;

    public class IntentParsingBusiness : IIntentParsingBusiness
    {
        public HomeCoachData GetDeviceData(IEnumerable<HomeCoachData> source, Dictionary<string, Slot> intentSlots)
        {
            if (intentSlots.ContainsKey("device"))
            {
                var deviceName = intentSlots["device"].Value.ToLower();

                var requestedDeviceData = source.FirstOrDefault(x => x.DeviceName.ToLower() == deviceName);
                if (requestedDeviceData == null)
                {
                    throw new DeviceNotFoundException();
                }

                return requestedDeviceData;
            }

            return source.First();
        }
    }

    public interface IIntentParsingBusiness
    {
        HomeCoachData GetDeviceData(IEnumerable<HomeCoachData> source, Dictionary<string, Slot> intentSlots);
    }
}