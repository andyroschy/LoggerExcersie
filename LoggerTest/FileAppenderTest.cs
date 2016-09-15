using BellatrixLogger;
using BellatrixLogger.Appenders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerTest
{
    [TestClass]
    public class FileAppenderTest
    {

        private static string LogFileDirectory { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestLogs"); } }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            if (!Directory.Exists(LogFileDirectory))
            {
                Directory.CreateDirectory(LogFileDirectory);
            }            
        }

        //[TestInitialize]
        //public void InitializeTest()
        //{
            
        //}

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void File_Appender_Should_Not_Work_Without_COnfiguration()
        {
            var fileAppender = new FileAppender(null);
        }

        [TestMethod]        
        public void File_Appender_Should_Create_File()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogDirectory).Returns(() => LogFileDirectory);            
            var fileAppender = new FileAppender(configMock.Object);
            Assert.IsTrue(!Directory.GetFiles(LogFileDirectory).Any());
            fileAppender.LogMessage("", default(LogLevel));
            Assert.IsTrue(Directory.GetFiles(LogFileDirectory).Any());
        }


        //Tests have a slight chance of failing if run when date changes
        [TestMethod]
        public void File_Appender_Multiple_Calls_Should_Not_Create_Multiple_Files()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogDirectory).Returns(() => LogFileDirectory);
            var fileAppender = new FileAppender(configMock.Object);
            Assert.IsTrue(!Directory.GetFiles(LogFileDirectory).Any());
            fileAppender.LogMessage("", default(LogLevel));
            fileAppender.LogMessage("", default(LogLevel));
            fileAppender.LogMessage("", default(LogLevel));
            Assert.IsTrue(Directory.GetFiles(LogFileDirectory).Count() == 1);
        }


        //Tests have a slight chance of failing if run when date changes
        [TestMethod]
        public void File_Appender_Should_Not_Overwrite_File_Content()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogDirectory).Returns(() => LogFileDirectory);
            var fileAppender = new FileAppender(configMock.Object);
            Assert.IsTrue(!Directory.GetFiles(LogFileDirectory).Any());            
            fileAppender.LogMessage("something", default(LogLevel));            
            var originalFileLength = File.ReadAllText(Directory.GetFiles(LogFileDirectory).First()).Length;            
            fileAppender.LogMessage("something else something something", default(LogLevel));
            var newFileLength = File.ReadAllText(Directory.GetFiles(LogFileDirectory).First()).Length;
            Assert.IsTrue(newFileLength > originalFileLength);
        }


        [TestCleanup]
        public void CleanupTest()
        {
            //delete all the files on the test directory
            var directory = new DirectoryInfo(LogFileDirectory);
            directory.GetFiles("*", SearchOption.AllDirectories).ToList().ForEach(file => file.Delete());
        }


        [ClassCleanup]
        public static void Cleanup()
        {            
            if (Directory.Exists(LogFileDirectory))
            {
                Directory.Delete(LogFileDirectory, true);
            }            
        }
    }
}
