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
            if (intentSlots != null && intentSlots.ContainsKey("device"))
            {
                var deviceName = intentSlots["device"].Value?.ToLower();

                if (deviceName != null)
                {
                    var requestedDeviceData = source.FirstOrDefault(x => x.DeviceName.ToLower() == deviceName);
                    if (requestedDeviceData == null)
                    {
                        var availableDevicesNames = source.Select(x => x.DeviceName).ToList();
                        throw new DeviceNotFoundException(deviceName, availableDevicesNames);
                    }

                    return requestedDeviceData;
                }
            }

            return source.First();
        }
    }
}