using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger.Extensions
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds a parameter to a db command
        /// </summary>
        /// <param name="command">The command to which the parameter will be added</param>
        /// <param name="parameterName">Name of the parameter to add</param>
        /// <param name="value">Value of the parameter</param>
        public static void AddDbParameter(this DbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
