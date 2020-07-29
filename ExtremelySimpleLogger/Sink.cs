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
        public LogLevel MinimumLevel { get; set; } = LogLevel.Info;
        /// <summary>
        /// The <see cref="LogFormatter"/> with which this message should be formatted.
        /// By default, <see cref="FormatDefault"/> is used.
        /// </summary>
        public LogFormatter Formatter { get; set; }
        /// <summary>
        /// If this property is set to <code>false</code>, this sink will not log any messages.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Initializes a new sink with the default settings.
        /// </summary>
        public Sink() {
            this.Formatter = this.FormatDefault;
        }

        /// <summary>
        /// Logs a message in the way specified by the sink's implementation.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public virtual void Log(Logger logger, LogLevel level, object message, Exception e = null) {
            this.Log(logger, level, this.Formatter.Invoke(logger, level, message, e));
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Formatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected abstract void Log(Logger logger, LogLevel level, string s);

        /// <summary>
        /// The default formatter for logging messages.
        /// By default, messages are laid out as follows:
        /// <code>
        /// [Date and time] [Logger name, if set] [Log level] Message
        ///    Exception, if set
        /// </code>
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        /// <returns>A formatted string to log</returns>
        protected virtual string FormatDefault(Logger logger, LogLevel level, object message, Exception e = null) {
            var builder = new StringBuilder();
            // date
            builder.Append($"[{DateTime.Now}] ");
            // logger name
            if (!string.IsNullOrEmpty(logger.Name))
                builder.Append($"[{logger.Name}] ");
            // log level
            builder.Append($"[{level}] ");
            // message
            builder.Append(message);
            // stack trace
            if (e != null)
                builder.Append($"\n{e}");
            return builder.ToString();
        }

        /// <summary>
        /// A delegate method used by <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public delegate string LogFormatter(Logger logger, LogLevel level, object message, Exception e = null);

        /// <summary>
        /// Disposes this sink, freeing all of the resources it uses.
        /// </summary>
        public virtual void Dispose() {
        }

    }
}