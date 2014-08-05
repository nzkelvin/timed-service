using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimedService;
using TimedService.Config;
using Moq;
using Microsoft.QualityTools.Testing.Fakes;
using TimedService.Logger;

namespace TimedService.UnitTests.TimedService_Tests
{
    [Trait("Test Type", "Unit Tests")]
    public class When_TimedService_CalculateRunAtTimeSpan
    {
        [Fact]
        public void Given_RunAtTimeIsLaterThanDateTimeNow_Then_RightTimeSpanIsCalculated()
        {
            // Arrange
            var mockConfigHelper = new Mock<IConfigurationHelper>();
            mockConfigHelper.Setup<TimedServicesConfig>(x => x.GetSection<TimedServicesConfig>()).Returns(new TimedServicesConfig { });

            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var timedService = new TimedServiceTestWrapper(mockConfigHelper.Object, mockLoggerFactory.Object);

            var inputDatetime = new DateTime(2014, 5, 4, 16, 0, 0);  // 4pm at 4/May/2014
            var expectedTimeSpan = new TimeSpan(5, 0, 0);

            // Act
            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2014, 5, 4, 11, 0, 0); // 11am at 4/May/2014
                var actualTimeSpan = timedService.CalculateRunAtTimeSpan(inputDatetime);

                // Assert
                Assert.Equal(expectedTimeSpan, actualTimeSpan);
            }
        }

        [Fact]    
        public void Given_RunAtTimeIsEarlierThanDateTimeNow_Then_RightTimeSpanIsCalculated()
        {
            // Arrange
            var mockConfigHelper = new Mock<IConfigurationHelper>();
            mockConfigHelper.Setup<TimedServicesConfig>(x => x.GetSection<TimedServicesConfig>()).Returns(new TimedServicesConfig { });

            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var timedService = new TimedServiceTestWrapper(mockConfigHelper.Object, mockLoggerFactory.Object);

            var inputDatetime = new DateTime(2014, 5, 4, 10, 0, 0);  // 10am at 4/May/2014. Only HH.MM.SS is used in the method
            var expectedTimeSpan = new TimeSpan(23, 0, 0);

            // Act
            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2014, 5, 4, 11, 0, 0); // 11am at 4/May/2014
                var actualTimeSpan = timedService.CalculateRunAtTimeSpan(inputDatetime);

                // Assert
                Assert.Equal(expectedTimeSpan, actualTimeSpan);
            }
        }
    }
}
