namespace HomeCoach.Business
{
    using System.Collections.Generic;
    using Alexa.NET.Request;
    using Models;

    public interface IIntentParsingBusiness
    {
        HomeCoachData GetDeviceData(IEnumerable<HomeCoachData> source, Dictionary<string, Slot> intentSlots);
    }
}