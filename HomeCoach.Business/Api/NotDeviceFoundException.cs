namespace HomeCoach.Business
{
    using System;
    using System.Collections.Generic;

    public class DeviceNotFoundException : Exception
    {
        public readonly string DeviceName;
        public readonly IEnumerable<string> FoundDevices;
        
        public DeviceNotFoundException(string deviceName):base()
        {
            this.DeviceName = deviceName;
        }      
        
        public DeviceNotFoundException(string deviceName, IEnumerable<string> foundDevices):base()
        {
            this.DeviceName = deviceName;
            this.FoundDevices = foundDevices;
        }
    }
}