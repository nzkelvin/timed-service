using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TimedService;
using TimedService.Config;
using TimedService.Logger;

namespace TimedService.UnitTests.TimedService_Tests
{
    public class When_TimedService_GetTimedCommandInstance
    {
        [Fact]
        public void Given_ValidTimedServiceCommandClass_Then_TimeCommandInstanceCreatedSuccessfully()
        {
            var mockConfigureHelper = new Mock<IConfigurationHelper>();
            mockConfigureHelper.Setup<TimedServicesConfig>(mch => mch.GetSection<TimedServicesConfig>()).Returns(new TimedServicesConfig()); // empty config is fine cos I only need the time service as a shell.
            
            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var timedServiceTestWrapper = new TimedServiceTestWrapper(mockConfigureHelper.Object, mockLoggerFactory.Object);

            var commandConfig = new TimedServiceCommandConfig{
                Name = "Test",
                Type = "TimedService.UnitTests.TimedServiceCommands.Test, TimedService.UnitTests",
                RunAt = DateTime.Today.AddHours(11) // 11 am today.
            };

            var expected = (new TimedServiceCommands.Test()).GetType();

            // Act
            var actual = timedServiceTestWrapper.GetTimedCommandInstance(commandConfig).GetType();
            
            // Assert
            Assert.Equal(expected, actual);

            //"TimedService.TimedServiceCommands.TestTimedService, TimedService"
        }
    }
}
