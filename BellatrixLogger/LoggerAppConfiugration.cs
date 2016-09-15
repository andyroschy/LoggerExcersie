using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace BellatrixLogger
{
    public class LoggerAppConfiugration : IConfiguration
    {

        public LogLevel LogLevel
        {
            get { return (LogLevel)Enum.Parse(typeof(LogLevel), ConfigurationManager.AppSettings["LogLevel"]); }
        }

        public ConnectionStringSettings DefaultConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DefaultConnection"]; }
        }

        public string LogDirectory
        {
            get { return ConfigurationManager.AppSettings["LogDirectory"]; }
        }


        public IEnumerable<string> ActiveAppenders
        {
            get { return ConfigurationManager.AppSettings["ActiveAppenders"].Split(','); }
        }
    }
}
