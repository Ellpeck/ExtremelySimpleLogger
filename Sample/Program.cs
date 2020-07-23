using System;
using System.Collections.Generic;
using System.IO;
using ExtremelySimpleLogger;

namespace Sample {
    internal static class Program {

        private static void Main() {
            // When we have multiple loggers that output to the same file,
            // we can just reuse our sinks
            var sinks = new List<Sink> {
                // We want to log messages of every log level, so we set the minimum level to Trace
                new FileSink("Log.txt", true) {MinimumLevel = LogLevel.Trace},
                new ConsoleSink()
            };
            var logger = new Logger {
                Name = "Example Logger",
                Sinks = sinks
            };
            var otherLogger = new Logger {
                Name = "Special Logger",
                Sinks = sinks
            };
            logger.Info("Logger loaded.");

            // Logging an exception
            logger.Warn("Unsafe code follows!");
            try {
                File.OpenRead("does/not/exist");
            } catch (Exception e) {
                logger.Error("An exception was thrown!", e);
            }

            otherLogger.Info("This is a special message from the special logger!");

            logger.Log(LogLevel.Trace, "This is a message that only the file sink will receive, since its minimum level is lower.");
            logger.Log(LogLevel.Info, "The program finished.");

            // Once we're done using the logger, we can dispose it so that our FileSink instances free their files
            logger.Dispose();
        }

    }
}