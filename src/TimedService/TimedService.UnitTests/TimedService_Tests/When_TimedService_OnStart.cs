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
    [Trait("Test Type", "Unit Tests")]
    public class When_TimedService_OnStart
    {
        [Fact]
        public void Given_OneTimedServiceCommandInTimedServicesConfig_Then_OneThreadingTimerCreated()
        {
            // Arrange
            var mockConfigHelper = new Mock<IConfigurationHelper>();
            mockConfigHelper.Setup<TimedServicesConfig>(x => x.GetSection<TimedServicesConfig>(It.IsAny<string>())).Returns(new TimedServicesConfig
            {
                TimedServiceCommandConfigs = new TimedServiceCommandConfig[] {
                    new TimedServiceCommandConfig{
                            Name = "TestCommand",
                            Type = "KelvinSoft.TestLib",
                            RunAt = DateTime.Today.AddHours(11) //11 am today
                    }
                }
            });

            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var timedService = new TimedServiceTestWrapper(mockConfigHelper.Object, mockLoggerFactory.Object);
            var expected = timedService.Timers.Count + 1; // The Timer is a static property of the dictionary type. Therefore it is a global count.

            // Act
            timedService.TriggerOnStart(new string[] { });

            // Assert
            var actual = timedService.Timers.Count;//TimedServiceTestWrapper._timers.Count; timedService.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Given_MultipleTimedServiceCommandsInTimedServicesConfig_Then_MultipleThreadingTimersCreated()
        {
            // Arrange
            var mockConfigHelper = new Mock<IConfigurationHelper>();
            mockConfigHelper.Setup<TimedServicesConfig>(x => x.GetSection<TimedServicesConfig>(It.IsAny<string>())).Returns(new TimedServicesConfig
            {
                TimedServiceCommandConfigs = new TimedServiceCommandConfig[] {
                    new TimedServiceCommandConfig{
                            Name = "TestCommand2",
                            Type = "KelvinSoft.TestLib",
                            RunAt = DateTime.Today.AddHours(11) //11 am today
                    },
                    new TimedServiceCommandConfig{
                            Name = "TestCommand3",
                            Type = "KelvinSoft.TestLib",
                            RunEvery = DateTime.Today.AddHours(17) //every 17 hours
                    }
                }
            });

            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var timedService = new TimedServiceTestWrapper(mockConfigHelper.Object, mockLoggerFactory.Object);
            var expected = timedService.Timers.Count + 2; // The Timer is a static property of the dictionary type. Therefore it is a global count.

            // Act
            timedService.TriggerOnStart(new string[] { });

            // Assert
            var actual = timedService.Timers.Count;//TimedServiceTestWrapper._timers.Count; timedService.
            Assert.Equal(expected, actual);
        }

        ///// <summary>
        ///// System.Thread.Timer has no property to see the initail due time and subsiquence interval time once the timer is setup.
        ///// Therefore I will test the CalculateRunAtTimeSpan method and CalculateRunEveryTimeSpan instead
        ///// </summary>
        //[Fact]
        //public void Given_TimedServiceCommandIsSetToRunAt_Then_ItsTimerIsSetToRunAt
        //{

        //}

        //[Fact]
        //public void Given_TimedServiceCommandIsSetToRunEvery_Then_ItsTimerIsSetToRunEvery
        //{

        //}

        /// <summary>
        /// Timers property contains a colletions of all running timers. We need to make sure there is only one collection.
        /// </summary>
        [Fact]
        public void Given_MultipleTimedServiceInstances_Then_ShareTheSameTimersPropertyObject()
        {
            // Arrange
            var mockConfigHelper = new Mock<IConfigurationHelper>();
            mockConfigHelper.Setup<TimedServicesConfig>(x => x.GetSection<TimedServicesConfig>()).Returns(new TimedServicesConfig { });

            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(mlf => mlf.ProduceLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            // Act
            var timers1 = new TimedServiceTestWrapper(mockConfigHelper.Object, mockLoggerFactory.Object).Timers;
            var timers2 = new TimedServiceTestWrapper(mockConfigHelper.Object, mockLoggerFactory.Object).Timers;

            // Assert
            Assert.Same(timers1, timers2);
        }
    }
}
