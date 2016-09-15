using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BellatrixLogger
{
    public interface IConfiguration
    {
        LogLevel LogLevel { get; }

        ConnectionStringSettings DefaultConnectionString { get; }

        string LogDirectory { get;  }

        IEnumerable<string> ActiveAppenders { get; }
    }
}
