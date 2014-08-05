using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimedService.Logger;
using System.IO;
using Microsoft.QualityTools.Testing.Fakes;

namespace TimedService.UnitTests.Logger.ConsoleLogger_Tests
{
    public class When_ConsoleLogger_LogException
    {
        [Fact]
        public void Given_ExceptionAndCustomMessage_Then_ExceptionLogFormattedCorrectly()
        {
            // Arrange
            var exception = new ApplicationException("Unit Test Exception", new ArgumentNullException("fakeParam"));

            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2014, 8, 2, 10, 9, 8);

                // Action
                var logger = new ConsoleLogger();
                logger.LogException(exception, "{0} test custom message", "My");

                // Assert
                var expectedLogMsg = "[14-08-02 10:09:08]Error: My test custom messageException Message: Unit Test Exception\r\nInner Exception: System.ArgumentNullException: Value cannot be null.\r\nParameter name: fakeParam\r\nStack Trace:\r\n";
                var actualLogMsg = logger.GetLoggedMessages();
                Assert.Equal(expectedLogMsg, actualLogMsg);
            }
        }
    }
}
