using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// The implementation of a logger, which is a wrapper class around multiple logging <see cref="Sinks"/>.
    /// To start logging with a logger, its <see cref="Sinks"/> need to be initialized.
    /// </summary>
    public class Logger : IDisposable {

        /// <summary>
        /// All of the <see cref="Sink"/> instances that this logger logs to.
        /// By default, <see cref="FileSink"/>, <see cref="ConsoleSink"/>, <see cref="DirectorySink"/> and <see cref="StringSink"/> are available.
        /// </summary>
        public List<Sink> Sinks { get; set; } = new List<Sink>();
        /// <summary>
        /// The minimum <see cref="LogLevel"/> that a message needs to have for this logger to log it.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Trace;
        /// <summary>
        /// The <see cref="LogFormatter"/> with which log messages should be formatted by a <see cref="Sink"/> if its <see cref="Sink.Formatter"/> is <see langword="null"/>.
        /// By default, <see cref="FormatDefault"/> is used.
        /// </summary>
        public LogFormatter DefaultFormatter { get; set; } = Logger.FormatDefault;
        /// <summary>
        /// If this property is set to <code>false</code>, this logger will not log any messages.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>
        /// The name of this logger. This name is used in <see cref="FormatDefault"/> by default.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new logger with the given name.
        /// Note that, for this logger to do anything, its <see cref="Sinks"/> need to be initialized.
        /// </summary>
        /// <param name="name">The logger's name</param>
        public Logger(string name = "") {
            this.Name = name;
        }

        /// <summary>
        /// Logs a message, passing it on to this logger's <see cref="Sinks"/>.
        /// </summary>
        /// <param name="level">The importance level of this message</param>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public void Log(LogLevel level, object message, Exception e = null) {
            if (!this.IsEnabled || level < this.MinimumLevel)
                return;
            foreach (var sink in this.Sinks) {
                if (sink.IsEnabled && level >= sink.MinimumLevel)
                    sink.Log(this, level, message, e);
            }
        }

        /// <summary>
        /// Logs a message with the <see cref="LogLevel.Trace"/> log level.
        /// </summary>
        /// <param name="message">The message</param>
        public void Trace(object message) => this.Log(LogLevel.Trace, message);

        /// <summary>
        /// Logs a message with the <see cref="LogLevel.Debug"/> log level.
        /// </summary>
        /// <param name="message">The message</param>
        public void Debug(object message) => this.Log(LogLevel.Debug, message);

        /// <summary>
        /// Logs a message with the <see cref="LogLevel.Info"/> log level.
        /// </summary>
        /// <param name="message">The message</param>
        public void Info(object message) => this.Log(LogLevel.Info, message);

        /// <summary>
        /// Logs a message with the <see cref="LogLevel.Warn"/> log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public void Warn(object message, Exception e = null) => this.Log(LogLevel.Warn, message, e);

        /// <summary>
        /// Logs a message with the <see cref="LogLevel.Error"/> log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public void Error(object message, Exception e = null) => this.Log(LogLevel.Error, message, e);

        /// <summary>
        /// Logs a message with the <see cref="LogLevel.Fatal"/> log level.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
        public void Fatal(object message, Exception e = null) => this.Log(LogLevel.Fatal, message, e);

        /// <summary>
        /// Disposes this logger, freeing all of the resources associated with its <see cref="Sinks"/>.
        /// </summary>
        public void Dispose() {
            foreach (var sink in this.Sinks)
                sink.Dispose();
        }

        /// <summary>
        /// The default formatter for logging messages, which is <see cref="DefaultFormatter"/>'s initial value.
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
        public static string FormatDefault(Logger logger, LogLevel level, object message, Exception e = null) {
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

    }

    /// <summary>
    /// A delegate method used by <see cref="Sink.Formatter"/> and <see cref="Logger.DefaultFormatter"/>.
    /// </summary>
    /// <param name="logger">The logger that the message was passed to</param>
    /// <param name="level">The importance level of this message</param>
    /// <param name="message">The message</param>
    /// <param name="e">An optional exception whose stack trace will be appended to the message</param>
    public delegate string LogFormatter(Logger logger, LogLevel level, object message, Exception e = null);
}