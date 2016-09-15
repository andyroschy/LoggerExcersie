using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger.Appenders
{
    public class ConsoleAppender : IAppender
    {
        private static object consoleLock = new object();

        private static Dictionary<LogLevel, ConsoleColor> consoleColorsPerLevel;

        static ConsoleAppender()
        {
            consoleColorsPerLevel = LevelMetadataAttribute.LogLevelsWithMetadata.ToDictionary(x => x.Key, x => x.Value.ConsoleLogColor);
        }


        public void LogMessage(string message, LogLevel messageLevel)
        {
            //locking to prevent issues with other threads changing the console color
            //since the console color can still be changed by outside code, 
            //in a real multithreaded console application, if i had the need to use diffrent text colors for diffrent outputs, 
            //i would probably not use the conosole class directly, but instead wrapit around a class that takes care of
            //synchronzing the messages across threads to ensure that the message is written with the correct color and other threads don't interfere
            lock (consoleLock)
            {
                Console.ForegroundColor = consoleColorsPerLevel[messageLevel];
                Console.WriteLine("LOG: Date: {0}, Message:{1}", DateTime.Now, message);
                Console.ResetColor();
            }
        }


        public string AppenderName
        {
            get { return "Console"; }
        }
    }
}
