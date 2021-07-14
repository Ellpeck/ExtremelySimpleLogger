using System;
using System.Collections.Generic;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to <see cref="Console.Out"/>.
    /// </summary>
    public class ConsoleSink : Sink {

        /// <summary>
        /// The <see cref="ConsoleColors"/> that each <see cref="LogLevel"/> is displayed with using this console sink.
        /// To edit and query this collection, use <see cref="SetColor"/> and <see cref="GetColor"/>.
        /// </summary>
        protected readonly Dictionary<LogLevel, ConsoleColor> ConsoleColors = new Dictionary<LogLevel, ConsoleColor> {
            {LogLevel.Warn, ConsoleColor.DarkYellow},
            {LogLevel.Error, ConsoleColor.DarkRed},
            {LogLevel.Fatal, ConsoleColor.DarkRed}
        };
        private readonly object locker = new object();

        /// <summary>
        /// Sets the <see cref="ConsoleColor"/> that text with the given <see cref="LogLevel"/> should be displayed with.
        /// To set the default color for the log level, simply pass a <see cref="Nullable{ConsoleColor}"/> with no value to <paramref name="color"/>.
        /// </summary>
        /// <param name="level">The log level to set the color for</param>
        /// <param name="color">The color to use, or a <see cref="Nullable{ConsoleColor}"/> with no value to clear the current color</param>
        public virtual void SetColor(LogLevel level, ConsoleColor? color) {
            if (color.HasValue) {
                this.ConsoleColors[level] = color.Value;
            } else {
                this.ConsoleColors.Remove(level);
            }
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
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected override void Log(Logger logger, LogLevel level, string s) {
            lock (this.locker) {
                var hasColor = this.ConsoleColors.TryGetValue(level, out var color);
                if (hasColor)
                    Console.ForegroundColor = color;
                Console.WriteLine(s);
                if (hasColor)
                    Console.ResetColor();
            }
        }

    }
}