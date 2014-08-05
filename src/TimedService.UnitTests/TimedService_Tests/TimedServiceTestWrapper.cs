using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedService;
using TimedService.Config;
using TimedService.Logger;

namespace TimedService.UnitTests.TimedService_Tests
{
    class TimedServiceTestWrapper : TimedService
    {
        public TimedServiceTestWrapper(IConfigurationHelper configHelper, ILoggerFactory loggerFactory)
            : base(configHelper, loggerFactory)
        {
        }

        public void TriggerOnStart(string[] args)
        {
            OnStart(args);
        }
    }
}
