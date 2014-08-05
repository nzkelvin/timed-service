using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimedService;
using TimedService.Config;
using Moq;
using TimedService.Logger;

namespace TimedService.UnitTests.TimedService_Tests
{
    public class When_TimedService_CalculateRunEveryTimeSpan
    {
        [Fact]
        public void Given_RunEveryTime_Then_CorrectRunEveryTimeSpanIsCalculated()
        {
            // Arrange
            var mockConfigureHelper = new Mock<IConfigurationHelper>();
            mockConfigureHelper.Setup<TimedServicesConfig>(mch => mch.GetSection<TimedServicesConfig>()).Returns(new TimedServicesConfig());

            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var timedServiceTestWrapper = new TimedServiceTestWrapper(mockConfigureHelper.Object, mockLoggerFactory.Object);

            var expectedResult = new TimeSpan(hours: 4, minutes: 25, seconds: 30);

            //Act
            var actualResult = timedServiceTestWrapper.CalculateRunEveryTimeSpan(new DateTime(year: DateTime.Today.Year,
                                                                            month: DateTime.Today.Month,
                                                                            day: DateTime.Today.Day,
                                                                            hour: 4,
                                                                            minute: 25,
                                                                            second: 30));

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
