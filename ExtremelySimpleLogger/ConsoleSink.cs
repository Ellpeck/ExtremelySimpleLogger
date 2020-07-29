using System;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to <see cref="Console.Out"/>.
    /// </summary>
    public class ConsoleSink : Sink {

        /// <summary>
        /// The <see cref="ConsoleColor"/> that this console sink should use when printing messages with the <see cref="LogLevel.Warn"/> log level.
        /// </summary>
        public ConsoleColor WarnColor { get; set; } = ConsoleColor.DarkYellow;
        /// <summary>
        /// The <see cref="ConsoleColor"/> that this console sink should use when printing messages with the <see cref="LogLevel.Error"/> and <see cref="LogLevel.Fatal"/> log levels.
        /// </summary>
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.DarkRed;
        private readonly object locker = new object();

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected override void Log(Logger logger, LogLevel level, string s) {
            lock (this.locker) {
                switch (level) {
                    case LogLevel.Warn:
                        Console.ForegroundColor = this.WarnColor;
                        break;
                    case LogLevel.Error:
                    case LogLevel.Fatal:
                        Console.ForegroundColor = this.ErrorColor;
                        break;
                }
                Console.WriteLine(s);
                Console.ResetColor();
            }
        }

    }
}