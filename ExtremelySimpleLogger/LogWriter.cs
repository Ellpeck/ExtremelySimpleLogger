using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// Implementation of a <see cref="TextWriter"/> that writes to a <see cref="Logger"/>.
    /// </summary>
    public class LogWriter : TextWriter {

        /// <summary>
        /// The log level that this log writer should write with.
        /// </summary>
        public LogLevel Level {
            get {
                lock (this.logger)
                    return this.level;
            }
            set {
                lock (this.logger)
                    this.level = value;
            }
        }

        /// <inheritdoc />
        public override Encoding Encoding => Encoding.UTF8;

        private readonly StringBuilder line = new StringBuilder();
        private readonly Logger logger;

        private LogLevel level;

        /// <summary>
        /// Creates a new log writer with the given settings.
        /// </summary>
        /// <param name="logger">The logger to write to.</param>
        /// <param name="level">The log level to write with.</param>
        public LogWriter(Logger logger, LogLevel level = LogLevel.Info) {
            this.logger = logger;
            this.level = level;
        }

        /// <inheritdoc />
        public override void Write(char value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(char[] buffer, int index, int count) {
            lock (this.logger)
                this.line.Append(buffer, index, count);
        }

        /// <inheritdoc />
        public override void Write(string value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void WriteLine(string value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine() {
            this.Flush();
        }

        /// <inheritdoc />
        public override void Flush() {
            lock (this.logger) {
                this.logger.Log(this.level, this.line);
                this.line.Clear();
            }
        }

    }
}
