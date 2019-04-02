namespace HomeCoach.Tests
{
    using System;
    using System.Collections.Generic;
    using Alexa.NET.Request;
    using Business;
    using Business.Models;
    using FluentAssertions;
    using Xunit;

    public class IntentParsingBusinessShould
    {
        [Fact]
        public void Return_First_Device_Given_No_Slot()
        {
            // Arrange
            var business = new IntentParsingBusiness();
            var expectedDevice = new HomeCoachData()
            {
                DeviceName = "Device 1"
            };
            var stubDevices = new List<HomeCoachData>()
            {
                expectedDevice,
                new HomeCoachData()
                {
                    DeviceName = "Device 2"
                }
            };
            // Act
            var result = business.GetDeviceData(stubDevices, null);

            // Assert
            result.Should().BeSameAs(expectedDevice);
        }       
        
        [Fact]
        public void Throws_Device_Not_Found_Exception_Given_Slot()
        {
            // Arrange
            var business = new IntentParsingBusiness();
            var expectedDevice = new HomeCoachData()
            {
                DeviceName = "Device 1"
            };
            var stubDevices = new List<HomeCoachData>()
            {
                expectedDevice,
                new HomeCoachData()
                {
                    DeviceName = "Device 2"
                }
            };
            var stubSlots = new Dictionary<string, Slot>();
            stubSlots.Add("device",new Slot(){Value = "unknown device"});
            
            // Act
            Action act = () =>  business.GetDeviceData(stubDevices, stubSlots);

            // Assert
            act.Should().Throw<DeviceNotFoundException>();
        }
        
        [Fact]
        public void Return_Requested_Device_Given_Slot()
        {
            // Arrange
            var business = new IntentParsingBusiness();
            var expectedDevice = new HomeCoachData()
            {
                DeviceName = "Device 3"
            };
            var stubDevices = new List<HomeCoachData>()
            {
                new HomeCoachData()
                {
                    DeviceName = "Device 1"
                },
                expectedDevice,
            };
            var stubSlots = new Dictionary<string, Slot>();
            stubSlots.Add("device",new Slot(){Value = expectedDevice.DeviceName});
            
            // Act
            var result = business.GetDeviceData(stubDevices, stubSlots);

            // Assert
            result.Should().BeSameAs(expectedDevice);
        }    
    }
}