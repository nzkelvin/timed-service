using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedService.Logger
{
    /// <summary>
    /// Note: Use IoC framework to enforce an single instance of this class when needed.
    /// </summary>
    public interface ILoggerFactory
    {
        ILogger ProduceLogger(string name);
    }
}
