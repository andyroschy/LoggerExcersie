using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using BellatrixLogger.Appenders;
using BellatrixLogger;

namespace LoggerTest
{
    [TestClass]
    public class ConsoleAppenderTest
    {
        [TestMethod]
        public void Appending_Messages_Should_Leave_Console_Forground_Intact()
        {
            var defaultColor = Console.ForegroundColor;
            var consoleAppender = new ConsoleAppender();
            consoleAppender.LogMessage("", LogLevel.Info);
            Assert.AreEqual(defaultColor, Console.ForegroundColor);
            consoleAppender.LogMessage("", LogLevel.Error);
            Assert.AreEqual(defaultColor, Console.ForegroundColor);
            consoleAppender.LogMessage("", LogLevel.Warning);
            Assert.AreEqual(defaultColor, Console.ForegroundColor);
        }

        [TestMethod]
        public void Appending_Messages_Should_Write_To_Console()
        {
            var outMock = new Mock<TextWriter>();
            outMock.Setup(x=> x.WriteLine(It.IsAny<string>(),It.IsAny<object>(),It.IsAny<object>())).
                Verifiable();
            Console.SetOut( outMock.Object);
            var consoleAppender = new ConsoleAppender();
            consoleAppender.LogMessage("", LogLevel.Info);
            outMock.Verify();
        }
    }
}
