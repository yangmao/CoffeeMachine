
using CoffeeMachine.Controllers;
using CoffeeMachine.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Models;
using Microsoft.Extensions.Logging;

namespace CoffeeMachineTests
{
    //To Test the controller workflow
    public class CoffeeControllerTests
    {
        private CoffeeController controller;
        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IWeatherService> mockWeatherService;
        private Mock<IUtilsService> mockUtilsService;
        private Mock<ILogger> mockLogger;
        public CoffeeControllerTests()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockConfiguration = new Mock<IConfiguration>();
            mockWeatherService = new Mock<IWeatherService>();
            mockUtilsService = new Mock<IUtilsService>();
            mockLogger = new Mock<ILogger>();
            controller = new CoffeeController(mockWeatherService.Object,mockUtilsService.Object,mockHttpClientFactory.Object,mockConfiguration.Object,mockLogger.Object);
        }

        [Fact]
        public void Given_NotFifthRequest_TemperatureLessThan30_Return_200_WithMessage()
        {
            mockWeatherService.Setup(x => x.GetCurrentTemperature(mockHttpClientFactory.Object, mockConfiguration.Object, mockLogger.Object)).ReturnsAsync(12);
            mockUtilsService.Setup(x => x.CountRequests(It.IsAny<HttpContext>(), It.IsAny<string>())).Returns(1);
            var result = controller.GetAsync().Result as OkObjectResult;
            var coffeeresult = result.Value as CoffeeResult;
            
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            coffeeresult.message.Should().Be("Your piping hot coffee is ready");
        }

        [Fact]
        public void Given_NotFifthRequest_TemperatureLessGreaterOrEqual30_Return_200_WithMessage()
        {
            mockWeatherService.Setup(x => x.GetCurrentTemperature(mockHttpClientFactory.Object, mockConfiguration.Object,mockLogger.Object)).ReturnsAsync(30);
            mockUtilsService.Setup(x => x.CountRequests(It.IsAny<HttpContext>(), It.IsAny<string>())).Returns(1);
            var result = controller.GetAsync().Result as OkObjectResult;
            var coffeeresult = result.Value as CoffeeResult;

            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            coffeeresult.message.Should().Be("Your refreshing iced coffee is ready");
        }

        [Fact]
        public void Given_FifthRequest_Return_503()
        {
            mockUtilsService.Setup(x => x.CountRequests(It.IsAny<HttpContext>(), It.IsAny<string>())).Returns(5);
            var result = controller.GetAsync().Result;
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(503);
        }

        [Fact]
        public void Given_OnFoolsDay_Return_418()
        {
            mockUtilsService.Setup(x => x.GetToday()).Returns("04-01");
            var result = controller.GetAsync().Result;
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(418);
        }

    }
}