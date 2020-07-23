using System;
using System.IO;
using ExtremelySimpleLogger;

namespace Sample {
    internal static class Program {

        private static void Main() {
            var logger = new Logger {
                Name = "Example Logger",
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

            logger.Log(LogLevel.Trace, "This is a message that only the file sink will receive, since its minimum level is lower.");
            logger.Log(LogLevel.Info, "The program finished.");
        }

    }
}