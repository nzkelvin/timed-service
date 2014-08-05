using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedService.Logger
{
    public class ConsoleLogger : BaseLogger
    {
        public override void Log(string message, params object[] args)
        {
            var timedMsg = FormatMessage(message, args);
            Console.WriteLine(timedMsg);
            _messageBuilder.AppendLine(timedMsg);
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
