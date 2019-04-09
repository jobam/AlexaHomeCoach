namespace HomeCoach.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alexa.NET.Request;
    using Alexa.NET.Request.Type;
    using Alexa.NET.Response;
    using Api.Controllers;
    using Business;
    using Business.Models;
    using Business.Response;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class IntentControllerShould
    {
        private Mock<INetatmoDataBusiness> business;
        private Mock<IResponseBusiness> responseBusiness;
        private Mock<IIntentParsingBusiness> intentParsingBusiness;
        private readonly IntentController controller;

        public IntentControllerShould()
        {
            this.business = new Mock<INetatmoDataBusiness>();
            this.responseBusiness = new Mock<IResponseBusiness>();
            this.intentParsingBusiness = new Mock<IIntentParsingBusiness>();

            this.controller = new IntentController(business.Object, responseBusiness.Object, intentParsingBusiness.Object);
        }

        [Trait("Category", "Devices")]
        [Fact]
        public async void Return_ConnectionRequest_When_No_netAtmoAccessToken()
        {
            // Arrange
            var request = new SkillRequest()
            {
                Request = new IntentRequest()
            };

            // Act
            var result = await this.controller.GetDevicesData(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeOfType<SkillResponse>();
        }

        [Fact]
        public async void Return_SkillResponse_Given_NoSlot()
        {
            // Arrange
            var request = new SkillRequest()
            {
                Request = new IntentRequest()
                {
                    Intent = new Intent()
                    {
                        Name = "Intent"
                    }
                },
                Context = new Context()
                {
                    System = new AlexaSystem()
                    {
                        User = new User()
                        {
                            AccessToken = "token"
                        }
                    }
                }
            };
            var expectedResult = new HomeCoachData()
            {
                DeviceName = "device 1"
            };

            this.business.Setup(x => x.GetDevicesData(It.IsAny<string>()))
                .ReturnsAsync(new List<HomeCoachData>() {expectedResult});
            this.intentParsingBusiness.Setup(x => x.GetDeviceData(
                    It.IsAny<IEnumerable<HomeCoachData>>(),
                    It.IsAny<Dictionary<string, Slot>>()))
                .Returns(expectedResult);

            this.responseBusiness.Setup(x => x.BuildResponse(
                    It.IsAny<HomeCoachData>(),
                    It.IsAny<string>()))
                .Returns("");

            // Act
            var result = await this.controller.GetDevicesData(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeOfType<SkillResponse>();
        }

        [Fact]
        public async void Return_SkillResponse_Given_NoDataException()
        {
            // Arrange
            var request = new SkillRequest()
            {
                Request = new IntentRequest()
                {
                    Intent = new Intent()
                    {
                        Name = "Intent"
                    }
                },
                Context = new Context()
                {
                    System = new AlexaSystem()
                    {
                        User = new User()
                        {
                            AccessToken = "token"
                        }
                    }
                }
            };


            this.business.Setup(x => x.GetDevicesData(It.IsAny<string>()))
                .ThrowsAsync(new NoDataException());

            // Act
            var result = await this.controller.GetDevicesData(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var castedResult = (result as OkObjectResult);
            
            castedResult.Value.Should().BeOfType<SkillResponse>();
            ((castedResult.Value as SkillResponse).Response.OutputSpeech as PlainTextOutputSpeech)
                .Text
                .Should()
                .Be("Impossible de récupérer les données depuis Netatmo");
        }      
        
        
        [Fact]
        public async void Return_SkillResponse_Given_NotFoundDevice()
        {
            // Arrange
            var request = new SkillRequest()
            {
                Request = new IntentRequest()
                {
                    Intent = new Intent()
                    {
                        Name = "Intent"
                    }
                },
                Context = new Context()
                {
                    System = new AlexaSystem()
                    {
                        User = new User()
                        {
                            AccessToken = "token"
                        }
                    }
                }
            };

            var expectedDeviceName = "Device 2";
            this.business.Setup(x => x.GetDevicesData(It.IsAny<string>()))
                .ReturnsAsync(new List<HomeCoachData>());
            this.intentParsingBusiness.Setup(x => x.GetDeviceData(
                    It.IsAny<IEnumerable<HomeCoachData>>(),
                    It.IsAny<Dictionary<string, Slot>>()))
                .Throws(new DeviceNotFoundException(expectedDeviceName));

            // Act
            var result = await this.controller.GetDevicesData(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var castedResult = (result as OkObjectResult);
            
            castedResult.Value.Should().BeOfType<SkillResponse>();
            ((castedResult.Value as SkillResponse).Response.OutputSpeech as PlainTextOutputSpeech)
                .Text
                .Should()
                .Be($"L'appareil {expectedDeviceName} n'a pas été trouvé. ");
        }     
        
        [Fact]
        public async void Return_SkillResponse_With_DevicesList_Given_NotFoundDevice()
        {
            // Arrange
            var request = new SkillRequest()
            {
                Request = new IntentRequest()
                {
                    Intent = new Intent()
                    {
                        Name = "Intent"
                    }
                },
                Context = new Context()
                {
                    System = new AlexaSystem()
                    {
                        User = new User()
                        {
                            AccessToken = "token"
                        }
                    }
                }
            };

            var expectedResponseMessage = "L'appareil Device1 n'a pas été trouvé. Les appareils suivants ont été trouvés: Device2;Device3;";
            this.business.Setup(x => x.GetDevicesData(It.IsAny<string>()))
                .ReturnsAsync(new List<HomeCoachData>());
            this.intentParsingBusiness.Setup(x => x.GetDeviceData(
                    It.IsAny<IEnumerable<HomeCoachData>>(),
                    It.IsAny<Dictionary<string, Slot>>()))
                .Throws(new DeviceNotFoundException("Device1", new List<string>(){"Device2","Device3"}));

            // Act
            var result = await this.controller.GetDevicesData(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var castedResult = (result as OkObjectResult);
            
            castedResult.Value.Should().BeOfType<SkillResponse>();
            ((castedResult.Value as SkillResponse).Response.OutputSpeech as PlainTextOutputSpeech)
                .Text
                .Should()
                .Be(expectedResponseMessage);
        }
    }
}