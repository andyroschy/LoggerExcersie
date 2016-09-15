using BellatrixLogger.Appenders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BellatrixLogger
{
    public class ConnectionFactory : IConnectionFactory
    {
        private IConfiguration configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            this.configuration = configuration;
        }

        public DbConnection CreateDbConnection()
        {
            return CreateDbConnection(configuration.DefaultConnectionString.ConnectionString);
        }

        public DbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
