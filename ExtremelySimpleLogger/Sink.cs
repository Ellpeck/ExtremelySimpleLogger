using System;
using System.Text;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A sink is a way for log messages passed to a <see cref="Logger"/> to be processed in a certain way.
    /// </summary>
    public abstract class Sink : IDisposable {

        /// <summary>
        /// The minimum level that a log message needs to have for it to be processed by this sink.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Trace;
        /// <summary>
        /// The <see cref="LogFormatter"/> with which log messages should be formatted.
        /// <see cref="Logger.DefaultFormatter"/> is used if this is <see langword="null"/>.
        /// </summary>
        public LogFormatter Formatter { get; set; }
        /// <summary>
        /// If this property is set to <code>false</code>, this sink will not log any messages.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Logs a message in the way specified by the sink's implementation.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public virtual void Log(Logger logger, LogLevel level, object message, Exception e = null) {
            var formatter = this.Formatter ?? logger.DefaultFormatter;
            this.Log(logger, level, formatter.Invoke(logger, level, message, e));
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Formatter"/> or <see cref="Logger.DefaultFormatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected abstract void Log(Logger logger, LogLevel level, string s);

        /// <summary>
        /// Disposes this sink, freeing all of the resources it uses.
        /// </summary>
        public virtual void Dispose() {}

    }
}