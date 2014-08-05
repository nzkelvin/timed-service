using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TimedService.Logger;
using System.IO;
using Microsoft.QualityTools.Testing.Fakes;

namespace TimedService.IntegrationTests.Logger.BasicLogger_Tests
{
    public class When_BasicLogger_Log
    {
        [Fact]
        public void Given_LogFileDoesNotAlreadyExist_Then_LogFileCreatedAndLogContentAdded()
        {
            // Arrange
            var logFilePath = @"TestLog.txt";
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            var loggerUnderTest = new BasicLogger(logFilePath);
            var expectedLogMessage = "[14-07-19 10:20:30]Test Line\r\n";

            // Act
            using(ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2014, 7, 19, 10, 20, 30);
                loggerUnderTest.Log("Test Line");
            }
            
            // Assert
            Assert.True(File.Exists(logFilePath));
            var actualLogMsg = File.ReadAllText(logFilePath);
            Assert.Equal(expectedLogMessage, actualLogMsg);

            // Tidy up
            File.Delete(logFilePath); // Comment this line out if you are not sure if the log file is created correctly.
        }
        

        //[Fact]
        //public void Given_LogFileAlreadyExists_Then_LogContentAppended()
        //{

        //}
    }
}
