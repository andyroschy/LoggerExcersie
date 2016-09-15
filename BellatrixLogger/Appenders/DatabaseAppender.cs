using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellatrixLogger.Extensions;

namespace BellatrixLogger.Appenders
{
    public class DatabaseAppender : IAppender
    {
        private IConnectionFactory connectionFactory;

        private static Dictionary<LogLevel, int> logLevelDatabaseIds;

        static DatabaseAppender()
        {
            logLevelDatabaseIds = LevelMetadataAttribute.LogLevelsWithMetadata.ToDictionary(x => x.Key, x => x.Value.DatabaseErrorTypeId);
        }

        public DatabaseAppender(IConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException("connectionFactory");
            }
            this.connectionFactory = connectionFactory;
        }

        
        public void LogMessage(string message, LogLevel messageLevel)
        {            
            using(var connection =  this.connectionFactory.CreateDbConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "Insert Into ApplicationLog Values (@message, @level,@date)";
                command.AddDbParameter("@message",message);                
                command.AddDbParameter("@level",logLevelDatabaseIds[messageLevel]);
                command.AddDbParameter("@date", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }


        public string AppenderName
        {
            get { return "Database"; }
        }
    }
}
