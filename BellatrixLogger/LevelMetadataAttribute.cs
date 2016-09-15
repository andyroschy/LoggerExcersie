using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger
{
    /// <summary>
    /// Metadata associated with the diffrent LogLevel values
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class LevelMetadataAttribute : Attribute
    {   
        /// <summary>
        /// The Console log color associated with this log level
        /// </summary>
        public ConsoleColor ConsoleLogColor { get; set; }

        /// <summary>
        /// The Database Id for the correspoding error type
        /// </summary>
        /// <remarks>
        /// Using this rather than the enum value to write in the database allows the flexibility to change them independently
        /// </remarks>
        public int DatabaseErrorTypeId { get; set; }

        private static Lazy<IReadOnlyDictionary<LogLevel, LevelMetadataAttribute>> logLevelsWithMetadata = new Lazy<IReadOnlyDictionary<LogLevel, LevelMetadataAttribute>>(GetLogLevelsWithMetadata);
        
        /// <summary>
        /// Gets all the LogLevels with their associated metadata
        /// </summary>
        /// <remarks>
        /// Uses a lazy implementantion so these values are cached in memory, and only created if needed. 
        /// </remarks>
        public static IReadOnlyDictionary<LogLevel, LevelMetadataAttribute> LogLevelsWithMetadata
        {
            get
            {                
                return logLevelsWithMetadata.Value;
            }
        }

        /// <summary>
        /// Reads all the enum values of LogLevel with their associated metadata. If no metadata is found for a LogLevel, a new metadata object is created
        /// with default values.
        /// </summary>
        /// <returns>A dictionary of log levels with associated metadata</returns>
        private static IReadOnlyDictionary<LogLevel, LevelMetadataAttribute> GetLogLevelsWithMetadata()
        {
            var levelsWithMetadata = new Dictionary<LogLevel, LevelMetadataAttribute>();
            var levelType = typeof(LogLevel);
            var enumNames = levelType.GetEnumNames();
            foreach (var name in enumNames)
            {
                var levelMetadata = levelType.
                                     GetMember(name).
                                     First().
                                     GetCustomAttributes(typeof(LevelMetadataAttribute), false).
                                     FirstOrDefault() as LevelMetadataAttribute;
                levelMetadata = levelMetadata ?? new LevelMetadataAttribute() { ConsoleLogColor = Constants.DefaultConsoleColor, DatabaseErrorTypeId = Constants.DefaultDatabaseErrorTypeId };
                levelsWithMetadata.Add((LogLevel)Enum.Parse(typeof(LogLevel), name), levelMetadata);
            }
            return levelsWithMetadata;
        }
    
    }
}
