using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TimedService.Logger;
using TimedService.Config;

namespace TimedService.UnitTests.Logger.BasicLoggerFactory_Tests
{
    public class When_BasicLoggerFactory_ProduceLogger
    {
        [Fact]
        public void Given_ProduceSameLoggerMultipleTimes_Then_SingleLoggerInstanceReturned()
        {
            // Arrange
            var mockConfigHelper = new Mock<IConfigurationHelper>();
            mockConfigHelper.Setup<TimedServicesConfig>(x => x.GetSection<TimedServicesConfig>("TimedServices")).Returns(new TimedServicesConfig { LogFilePath = "MockFilePath.log" });

            var dummyLoggerName = "dummyLogger";
            BasicLoggerFactory loggerFactory = new BasicLoggerFactory(mockConfigHelper.Object);
            
            // Action
            var logger = loggerFactory.ProduceLogger(dummyLoggerName);
            var logger2 = loggerFactory.ProduceLogger(dummyLoggerName);

            // Assert
            Assert.Same(logger, logger2);
        }
    }
}
