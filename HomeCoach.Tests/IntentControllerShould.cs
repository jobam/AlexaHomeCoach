namespace HomeCoach.Tests
{
    using Alexa.NET.Request;
    using Alexa.NET.Request.Type;
    using Alexa.NET.Response;
    using Api.Controllers;
    using Business;
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

        [Fact]
        public void Works()
        {
            var foo = true;

            foo.Should().BeTrue();
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
    }
}