using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedService.Logger;

namespace TimedService
{
    public interface ITimedServiceCommand
    {
        void OnTimedService(string name, Dictionary<string, string> parameters, ILogger logger);
    }
}
