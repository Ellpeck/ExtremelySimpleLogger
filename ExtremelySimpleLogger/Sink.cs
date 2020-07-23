using System;
using System.Text;

namespace ExtremelySimpleLogger {
    public abstract class Sink {

        public LogLevel MinimumLevel { get; set; } = LogLevel.Info;
        public LogFormatter Formatter { get; set; }

        public Sink() {
            this.Formatter = this.FormatDefault;
        }

        public void Log(Logger logger, LogLevel level, object message, Exception e = null) {
            this.Log(this.Formatter.Invoke(logger, level, message, e));
        }

        public abstract void Log(string s);

        public virtual string FormatDefault(Logger logger, LogLevel level, object message, Exception e = null) {
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
                builder.Append($"\n{e.StackTrace}");
            return builder.ToString();
        }

        public delegate string LogFormatter(Logger logger, LogLevel level, object message, Exception e = null);

    }
}