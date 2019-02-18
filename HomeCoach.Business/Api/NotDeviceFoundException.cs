namespace HomeCoach.Business
{
    using System;
    public class DeviceNotFoundException : Exception
    {
        public readonly string DeviceName;
        
        public DeviceNotFoundException(string deviceName):base()
        {
            this.DeviceName = deviceName;
        }
    }
}