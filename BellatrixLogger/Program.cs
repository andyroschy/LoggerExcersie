using BellatrixLogger.Appenders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger
{
    public class Program
    {
        static void Main(string[] args)
        {            
            var configuration = new LoggerAppConfiugration();
            var consoleAppender = new ConsoleAppender();
            var textAppender = new FileAppender(configuration);
            var databaseAppender = new DatabaseAppender(new ConnectionFactory(configuration));

            var logger = new Logger(new IAppender[] { consoleAppender, textAppender, databaseAppender }, configuration);

            logger.Log("This is information", LogLevel.Info);
            logger.Log("This is a warning", LogLevel.Warning);
            logger.Log("This is an error", LogLevel.Error);



            int i = 0;
            while (Console.ReadKey().Key != ConsoleKey.N)
            {
                i++;
                logger.Log("This is information " + i, LogLevel.Info);
                logger.Log("This is a warning " + i, LogLevel.Warning);
                logger.Log("This is an error " + i, LogLevel.Error);
            }
        }


    }
}
