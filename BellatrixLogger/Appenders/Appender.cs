using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BellatrixLogger.Appenders
{
    public interface IAppender
    {
        void LogMessage(string message, LogLevel messageLevel);
        string AppenderName { get; }
    }
}
