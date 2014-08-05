using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedService.Logger
{
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger ProduceLogger(string name)
        {
            return new ConsoleLogger(); // only a single logger here, which logs to the console window so no log name is necessary here.
        }
    }
}
