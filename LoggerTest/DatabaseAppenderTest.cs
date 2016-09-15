using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BellatrixLogger.Appenders;
using Moq;
using System.Data.Common;

namespace LoggerTest
{
    [TestClass]
    public class DatabaseAppenderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Database_Appender_Should_Not_Initialize_Without_COnnection_Factory()
        {            
            var databaseAppender = new DatabaseAppender(null);            
        }

        //Another test to corroborate that rows are inserted on a database should be added.
        //For this i coulde use DbUnit.Net, but that would require NUnit, and i've already wrote the test with MsTest so...

    }
}
