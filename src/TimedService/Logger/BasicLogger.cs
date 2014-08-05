using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TimedService.Logger
{
    /// <summary>
    /// LogFileName prop is used to indicate which file to log to. It can be set in constructor and property
    /// http://stackoverflow.com/questions/4809843/net-2-0-file-appendalltext-thread-safe-implementation
    /// File.WriteAllText() is thread safe because it's FileShare = Read. But multiple parallel processes will cause interleave log entries.
    /// Locker here can only prevent interleaved logging from within the same logger instance. Logger factory will make sure only a single instance will be generated.
    /// </summary>
    public class BasicLogger : BaseLogger
    {
        public string LogFileName { get; private set; }

        private object _lockerObj = new object();

        public BasicLogger(string logFileName)
        {
            this.LogFileName = logFileName;
        }

        public override void Log(string message, params object[] args)
        {
            lock (_lockerObj)
            {
                var timedMsg = FormatMessage(message, args);
                if(!String.IsNullOrWhiteSpace(LogFileName))
                {
                    File.AppendAllLines(LogFileName, new[] { timedMsg }); // AppendAllLines = AppendAllText + NewLine.
                }
                _messageBuilder.AppendLine(timedMsg);
            }
        }

        public override void LogException(Exception exception)
        {
            Log(FormatExceptionMessage(exception));
        }

        public override void LogException(Exception exception, string message, params object[] args)
        {
            Log(FormatExceptionMessage(exception, message, args));
        }
    }
}
