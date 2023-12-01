using System;
using System.Collections.Generic;
using System.IO;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to <see cref="Console.Out"/> or <see cref="Console.Error"/>.
    /// This class is a variation of the <see cref="WriterSink"/>.
    /// </summary>
    public class ConsoleSink : Sink {

        /// <summary>
        /// The <see cref="ConsoleColors"/> that each <see cref="LogLevel"/> is displayed with using this console sink.
        /// To edit and query this collection, you can also use <see cref="SetColor"/> and <see cref="GetColor"/>.
        /// </summary>
        public readonly Dictionary<LogLevel, ConsoleColor> ConsoleColors = new Dictionary<LogLevel, ConsoleColor> {
            {LogLevel.Warn, ConsoleColor.DarkYellow},
            {LogLevel.Error, ConsoleColor.DarkRed},
            {LogLevel.Fatal, ConsoleColor.DarkRed}
        };
        private readonly TextWriter console;
        private readonly bool useAnsiCodes;

        /// <summary>
        /// Creates a new console sink with the given settings.
        /// </summary>
        /// <param name="error">Whether to log to <see cref="Console.Error"/> instead of <see cref="Console.Out"/>.</param>
        /// <param name="useAnsiCodes">Whether to wrap log output text in ANSI escape codes using <see cref="Extensions.WrapAnsiCode"/> instead of using the <see cref="Console.ForegroundColor"/> and <see cref="Console.ResetColor"/>. This may work better on some terminals.</param>
        public ConsoleSink(bool error = false, bool useAnsiCodes = false) {
            this.console = error ? Console.Error : Console.Out;
            this.useAnsiCodes = useAnsiCodes;
        }

        /// <summary>
        /// Sets the <see cref="ConsoleColor"/> that text with the given <see cref="LogLevel"/> should be displayed with.
        /// To set the default color for the log level, simply pass a <see cref="Nullable{ConsoleColor}"/> with no value to <paramref name="color"/>.
        /// </summary>
        /// <param name="level">The log level to set the color for</param>
        /// <param name="color">The color to use, or a <see cref="Nullable{ConsoleColor}"/> with no value to clear the current color</param>
        /// <returns>This instance, for chaining</returns>
        public virtual ConsoleSink SetColor(LogLevel level, ConsoleColor? color) {
            if (color.HasValue) {
                this.ConsoleColors[level] = color.Value;
            } else {
                this.ConsoleColors.Remove(level);
            }
            return this;
        }

        /// <summary>
        /// Returns the <see cref="ConsoleColor"/> that text with the given <see cref="LogLevel"/> is displayed with.
        /// If text is displayed with the default console color, a <see cref="Nullable{ConsoleColor}"/> without a value is returned.
        /// </summary>
        /// <param name="level">The log level whose color to query</param>
        /// <returns>The console color that text with the log level is displayed with, or a <see cref="Nullable{ConsoleColor}"/> with no value if no color is set</returns>
        public virtual ConsoleColor? GetColor(LogLevel level) {
            if (this.ConsoleColors.TryGetValue(level, out var color))
                return color;
            return null;
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/> or <see cref="Logger.DefaultFormatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected override void Log(Logger logger, LogLevel level, string s) {
            lock (this.console) {
                var color = this.GetColor(level);
                if (color.HasValue) {
                    if (this.useAnsiCodes) {
                        s = s.WrapAnsiCode(color.Value);
                    } else {
                        Console.ForegroundColor = color.Value;
                    }
                }
                this.console.WriteLine(s);
                if (color.HasValue && !this.useAnsiCodes)
                    Console.ResetColor();
            }
        }

    }
}
