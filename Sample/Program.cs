using System;
using System.IO;
using ExtremelySimpleLogger;

namespace Sample {
    internal static class Program {

        private static void Main(string[] args) {
            var logger = new Logger {
                Name = "Test Logger",
                Sinks = {
                    new FileSink("Log.txt", true) {MinimumLevel = LogLevel.Trace}, 
                    new ConsoleSink()
                }
            };
            logger.Info("Logger loaded.");
            logger.Info("Program starting.");

            logger.Warn("Unsafe code follows!");
            try {
                File.OpenRead("does/not/exist");
            } catch (Exception e) {
                logger.Error("An exception was thrown!", e);
            }

            logger.Log(LogLevel.Trace, "The program finished.");
        }

    }
}