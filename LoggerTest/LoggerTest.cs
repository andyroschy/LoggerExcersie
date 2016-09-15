using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BellatrixLogger;
using BellatrixLogger.Appenders;
using System.Collections.Generic;

namespace LoggerTest
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void Logger_Should_Delegate_To_Appenders()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogLevel).Returns(() => LogLevel.Info);
            configMock.Setup(x => x.ActiveAppenders).Returns(() => new string[] { "mockapender" });
            var appenderMock = new Mock<IAppender>();
            appenderMock.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Verifiable("Log message was not called");
            appenderMock.Setup(x => x.AppenderName).Returns(() => "mockapender");
            var logger = new Logger(new IAppender[] { appenderMock.Object }, configMock.Object);
            logger.Log("something", LogLevel.Error);
            appenderMock.Verify();
        }

        [TestMethod]
        public void If_One_Appender_Fails_Others_Should_Not_Be_Affected()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogLevel).Returns(() => LogLevel.Info);
            configMock.Setup(x => x.ActiveAppenders).Returns(() => new string[] { "mockapender", "mockapender2" });
            var appenderMock = new Mock<IAppender>();
            appenderMock.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Throws(new Exception()).
                Verifiable();
            appenderMock.Setup(x => x.AppenderName).Returns(() => "mockapender");
            var appenderMock2 = new Mock<IAppender>();
            appenderMock2.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Verifiable("Log message was not called");
            appenderMock2.Setup(x => x.AppenderName).Returns(() => "mockapender2");
            var logger = new Logger(new IAppender[] { appenderMock.Object, appenderMock2.Object }, configMock.Object);
            logger.Log("something", LogLevel.Error);
            appenderMock.Verify();
            appenderMock2.Verify();
        }

        [TestMethod]
        public void Logger_Should_Only_Call_Active_Appenders()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogLevel).Returns(() => LogLevel.Info);
            configMock.Setup(x => x.ActiveAppenders).Returns(() => new string[] { "mockapender" });
            var appenderMock = new Mock<IAppender>();
            appenderMock.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Throws(new Exception()).
                Verifiable();
            appenderMock.Setup(x => x.AppenderName).Returns(() => "mockapender");
            var appenderMock2 = new Mock<IAppender>();
            appenderMock2.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Verifiable("Log message was not called");
            appenderMock2.Setup(x => x.AppenderName).Returns(() => "mockapender2");
            var logger = new Logger(new IAppender[] { appenderMock.Object, appenderMock2.Object }, configMock.Object);
            logger.Log("something", LogLevel.Error);
            appenderMock.Verify();
            appenderMock2.Verify(x=> x.LogMessage(It.IsAny<string>(),It.IsAny<LogLevel>()),Times.Never());
        }


        [TestMethod]
        public void Logger_Should_Not_Log_Messages_Below_Tresshold()
        {
            //paramater names not necesary but addd clarity 
            VeryfyMessageDosNotGetLogged(messageLevel: LogLevel.Info, tresholdLevel: LogLevel.Warning);
            VeryfyMessageDosNotGetLogged(messageLevel: LogLevel.Info, tresholdLevel: LogLevel.Error);
            VeryfyMessageDosNotGetLogged(messageLevel: LogLevel.Warning, tresholdLevel: LogLevel.Error);
        }

        private static void VeryfyMessageDosNotGetLogged(LogLevel messageLevel, LogLevel tresholdLevel)
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogLevel).Returns(() => tresholdLevel);
            var appenderMock = new Mock<IAppender>();
            appenderMock.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Throws(new Exception()).
                Verifiable();
            var logger = new Logger(new IAppender[] { appenderMock.Object }, configMock.Object);
            logger.Log("something", messageLevel);
            appenderMock.Verify(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>()), Times.Never());
        }

        [TestMethod]
        public void Logger_Should_Not_Log_EmptyMessages()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.LogLevel).Returns(() => LogLevel.Info);
            var appenderMock = new Mock<IAppender>();
            appenderMock.Setup(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>())).
                Throws(new Exception()).
                Verifiable();
            var logger = new Logger(new IAppender[] { appenderMock.Object }, configMock.Object);
            logger.Log(string.Empty, LogLevel.Error);
            logger.Log(null, LogLevel.Error);
            logger.Log("      ", LogLevel.Error);
            appenderMock.Verify(x => x.LogMessage(It.IsAny<string>(), It.IsAny<LogLevel>()), Times.Never());
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Logger_Should_Not_Initialize_Without_Appender()
        {
            var logger = new Logger(null, Mock.Of<IConfiguration>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Logger_Should_Not_Initialize_Without_Configuration()
        {
            var logger = new Logger(new IAppender[] { Mock.Of<IAppender>() }, null);
        }
    }
}
