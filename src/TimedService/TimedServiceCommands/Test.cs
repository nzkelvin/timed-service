using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedService.Logger;

namespace TimedService.TimedServiceCommands
{
    public class Test : ITimedServiceCommand
    {
        public void OnTimedService(string name, Dictionary<string, string> parameters, ILogger logger)
        {
            logger.Log("Test timed service command is run.");
        }
    }
}
