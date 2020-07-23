using System;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to <see cref="Console.Out"/>.
    /// </summary>
    public class ConsoleSink : Sink {

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="s">The message to log</param>
        public override void Log(string s) {
            Console.WriteLine(s);
        }

    }
}