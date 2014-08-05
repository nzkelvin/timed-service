using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedService.Logger
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
        void LogException(Exception exception);
        void LogException(Exception exception, string message, params object[] args);
        string GetLoggedMessages();
        void ClearLoggedMessages();
        //void Finalize();
    }
}
