using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger.Appenders
{
    public class FileAppender : IAppender
    {
        private IConfiguration configuration;
        private static object writeLock = new object();

        public FileAppender(IConfiguration configuratin)
        {
            if (configuratin == null)
            {
                throw new ArgumentNullException("configuratin");
            }
            this.configuration = configuratin;
        }

        public void LogMessage(string message, LogLevel messageLevel)
        {
            var filePath = Path.Combine(configuration.LogDirectory, string.Format("{0}-log.txt", DateTime.Now.ToString("yyyy-MM-dd")));
            lock (writeLock)
            {
                using (var writer = new StreamWriter(filePath,true))
                {
                    //Not using to short date string since time of the message could be useful
                    writer.WriteLine("LOG: {0} - {1} - {2}", DateTime.Now, message, messageLevel.ToString());
                    writer.Flush();                    
                }
            }
        }


        public string AppenderName
        {
            get { return "File"; }
        }
    }
}
