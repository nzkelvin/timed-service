using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedService.Config;
using Xunit;

namespace TimedService.IntegrationTests.Config.ConfigurationHelper_Tests
{
    [Trait("Test Type", "Integration Tests")]
    public class When_ConfigurationHelper_GetSection
    {
        [Fact]
        public void Given_ValidTimedServiceConfigurationXml_Then_ConfigurationXmlDeserialisedToObjects()
        {
            // Arrange
            IConfigurationHelper configHelper = new ConfigurationHelper();

            // Action
            var result = configHelper.GetSection<TimedServicesConfig>("TimedServices");

            // Assert
            Assert.True(result.TimedServiceCommandConfigs.Count() > 0);
        }

        [Fact]
        public void Given_RunAtTimeFormatHHMMSS_With_NoDateDefined_Then_TodaysDateWillBeUsedAsDefault()
        {
            // Arrange
            IConfigurationHelper configHelper = new ConfigurationHelper();
            var expectedResult = DateTime.Today.Date;

            // Action
            var section = configHelper.GetSection<TimedServicesConfig>("TimedServices");
            var actualResult = section.TimedServiceCommandConfigs.Where(c => c.Name == "TestTimedService").FirstOrDefault().RunAt.Value.Date;

            // Assert
            Assert.Equal<DateTime>(expectedResult, actualResult);
        }
    }
}
