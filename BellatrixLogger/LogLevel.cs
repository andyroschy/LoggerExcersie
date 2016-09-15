using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellatrixLogger
{
    public enum LogLevel
    {
        //Using difrent values for the id and the enum just to show the flexibility granted by using this metadata approach.
        [LevelMetadata(ConsoleLogColor = ConsoleColor.Gray, DatabaseErrorTypeId = 1)]
        Info = 0,
        [LevelMetadata(ConsoleLogColor = ConsoleColor.Yellow, DatabaseErrorTypeId = 2)]
        Warning = 1,
        [LevelMetadata(ConsoleLogColor = ConsoleColor.Red, DatabaseErrorTypeId = 3)]
        Error = 2
    }
}
