using System;
using System.Collections.Generic;

namespace ExtremelySimpleLogger {
    public class Logger {

        public List<Sink> Sinks { get; set; } = new List<Sink>();
        public LogLevel MinimumLevel { get; set; } = LogLevel.Trace;
        public bool IsEnabled { get; set; } = true;
        public string Name { get; set; }

        public Logger(string name = "") {
            this.Name = name;
        }

        public void Log(LogLevel level, object message, Exception e = null) {
            if (!this.IsEnabled || level < this.MinimumLevel)
                return;
            foreach (var sink in this.Sinks) {
                if (level >= sink.MinimumLevel)
                    sink.Log(this, level, message, e);
            }
        }

        public void Trace(object message) => this.Log(LogLevel.Trace, message);
        public void Debug(object message) => this.Log(LogLevel.Debug, message);
        public void Info(object message) => this.Log(LogLevel.Info, message);
        public void Warn(object message, Exception e = null) => this.Log(LogLevel.Warn, message, e);
        public void Error(object message, Exception e = null) => this.Log(LogLevel.Error, message, e);
        public void Fatal(object message, Exception e = null) => this.Log(LogLevel.Fatal, message, e);

    }
}