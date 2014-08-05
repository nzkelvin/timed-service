using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedService;
using TimedService.Logger;

namespace TimedService.UnitTests.TimedServiceCommands
{
    public class Test : ITimedServiceCommand
    {
        public void OnTimedService(string name, Dictionary<string, string> parameters, ILogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
