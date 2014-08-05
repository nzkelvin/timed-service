using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedService.Logger
{
    public abstract class BaseLogger : ILogger
    {
        protected StringBuilder _messageBuilder = new StringBuilder();

        public abstract void Log(string message, params object[] args);

        public abstract void LogException(Exception exception);

        public abstract void LogException(Exception exception, string message, params object[] args);

        protected string FormatMessage(string message, params object[] args)
        {
            var timeStamp = DateTime.Now;

            var formattedMsg = String.Format(message, args);
            var timedMsg = String.Format("[{0:yy-MM-dd HH:mm:ss}]", timeStamp);
            timedMsg += formattedMsg;
            return timedMsg;
        }

        protected string FormatExceptionMessage(Exception exception)
        {
            return String.Format("Exception Message: {0}\r\nInner Exception: {1}\r\nStack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace);
        }

        protected string FormatExceptionMessage(Exception exception, string message, params object[] args)
        {
            var messageString = String.IsNullOrWhiteSpace(message) ? String.Empty : String.Format("Error: " + message, args);
            var exceptionString = exception != null ? String.Format("Exception Message: {0}\r\nInner Exception: {1}\r\nStack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace) : String.Empty;
            return String.Format("{0}{1}{2}", messageString, (String.IsNullOrEmpty(messageString) && String.IsNullOrEmpty(exceptionString)) ? "\r\n" : String.Empty , exceptionString);
        }

        /// <summary>
        /// Get all messages since the 1st message or last Finalize() called.
        /// This is useful when you want to collect logs for an action like email out all logs.
        /// </summary>
        /// <returns></returns>
        public string GetLoggedMessages()
        {
            return _messageBuilder.ToString();
        }

        /// <summary>
        /// Compiler Warning (level 1) CS0465: If such a class is used as a base class and if the deriving class defines a destructor, 
        /// the destructor will override the base class Finalize method, not Finalize.
        /// </summary>
        public void ClearLoggedMessages()
        {
            _messageBuilder.Clear();
        }
    }
}
