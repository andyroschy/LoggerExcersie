using BellatrixLogger.Appenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger
{
    public class Logger
    {
        private IEnumerable<IAppender> appenders;
        private IConfiguration configuration;


        public Logger(IEnumerable<IAppender> appenders, IConfiguration configuration)
        {
            if (appenders == null)
            {
                throw new ArgumentNullException("appenders", "Appenders cannot be null. If no appenders need to be active, the argument should be an IEnumerable with no elements.");
            }
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            this.appenders = appenders;
            this.configuration = configuration;
        }

        private IEnumerable<IAppender> ActiveAppenders
        {
            get
            {
                return appenders.Where(appender => configuration.ActiveAppenders.Any(activeAppender => string.Compare(activeAppender,appender.AppenderName,true) == 0));
            }

        }

        /// <summary>
        /// Logs a message to the configured destinations, depending on the configured log level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageLevel"></param>
        public void Log(string message, LogLevel messageLevel)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            if (configuration.LogLevel > messageLevel) return;
            foreach (var appender in this.ActiveAppenders)
            {
                //If one appender fails, it should not affect the rest. Error handling at the logger level so i don't have to write error handling code in each appender.
                try
                {
                    appender.LogMessage(message, messageLevel);
                }                   
                catch (Exception ex)
                {
                    //The logger should never throw exceptions. In case something goes wrong with the logger itself                
                    //using the trace API we still have some possibility of knowign that something happend
                    //without risking an exception.                    
                    Trace.TraceError("An error ocurred while attempting to log a message.\n{0}", ex.Message);
                }
            }            
        }


    }
}
