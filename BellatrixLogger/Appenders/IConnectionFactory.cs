using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BellatrixLogger.Appenders
{
    public interface IConnectionFactory
    {
        DbConnection CreateDbConnection();
        DbConnection CreateDbConnection(string connectionString);
    }
}
