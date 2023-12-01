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
                new FileSink("Log.txt", true),
                // We only want to log messages with a higher importance in the console
                new ConsoleSink() {MinimumLevel = LogLevel.Info},
                // we allow a total of 5 files in our directory sink before old ones start being deleted
                new DirectorySink("AllLogs", 5)
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

            // we can also use a writer to write to the log
            Console.SetError(new LogWriter(logger, LogLevel.Warn));
            Console.Error.WriteLine("This is an error written through serr! Oh no!");
            Console.Error.Write("This is another error, but ");
            Console.Error.WriteLine("written in multiple parts!");

            // Once we're done using the logger, we can dispose it so that our FileSink instances free their files
            logger.Dispose();
        }

    }
}
