using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedService.Config;

namespace TimedService.Logger
{
    public class BasicLoggerFactory : ILoggerFactory
    {
        private IDictionary<string, ILogger> _loggerRepository = new Dictionary<string, ILogger>();
        private object _lockerObj = new object();
        private string _logFilePath;

        public BasicLoggerFactory(IConfigurationHelper configurationHelper)
        {
            _logFilePath = configurationHelper.GetSection<TimedServicesConfig>("TimedServices").LogFilePath;
        }

        /// <summary>
        /// If loggers names are the same, a single logger instance will be returned.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILogger ProduceLogger(string name)
        {
            var filePath = string.Format(_logFilePath, name);

            if (!_loggerRepository.ContainsKey(filePath))
            {
                lock (_lockerObj)
                {
                    if (!_loggerRepository.ContainsKey(filePath))
                    {
                        _loggerRepository.Add(filePath, new BasicLogger(filePath));
                    }
                }
            }

            return _loggerRepository[filePath];
        }
    }
}
