using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// Implementation of a <see cref="TextWriter"/> that writes to a <see cref="Logger"/>.
    /// The log writer constructs a message based on calls to <see cref="Write(string)"/> and its variations and then submits the message to the underlying <see cref="Logger"/> when <see cref="WriteLine()"/> or <see cref="Flush"/> is called. <see cref="WriteLine(string)"/> and its variations submit the message immediately.
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
        public override void Write(char[] buffer) {
            lock (this.logger)
                this.line.Append(buffer);
        }

        /// <inheritdoc />
        public override void Write(bool value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(int value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(uint value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(long value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(ulong value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(float value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(double value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(decimal value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(object value) {
            lock (this.logger)
                this.line.Append(value);
        }

        /// <inheritdoc />
        public override void Write(string format, object arg0) {
            lock (this.logger)
                this.line.AppendFormat(this.FormatProvider, format, arg0);
        }

        /// <inheritdoc />
        public override void Write(string format, object arg0, object arg1) {
            lock (this.logger)
                this.line.AppendFormat(this.FormatProvider, format, arg0, arg1);
        }

        /// <inheritdoc />
        public override void Write(string format, object arg0, object arg1, object arg2) {
            lock (this.logger)
                this.line.AppendFormat(this.FormatProvider, format, arg0, arg1, arg2);
        }

        /// <inheritdoc />
        public override void Write(string format, params object[] arg) {
            lock (this.logger)
                this.line.AppendFormat(this.FormatProvider, format, arg);
        }

        /// <inheritdoc />
        public override void WriteLine(string value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(char value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(char[] buffer) {
            this.Write(buffer);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(char[] buffer, int index, int count) {
            this.Write(buffer, index, count);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(bool value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(int value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(uint value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(long value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(ulong value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(float value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(double value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(decimal value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(object value) {
            this.Write(value);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(string format, object arg0) {
            this.Write(format, arg0);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(string format, object arg0, object arg1) {
            this.Write(format, arg0, arg1);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(string format, object arg0, object arg1, object arg2) {
            this.Write(format, arg0, arg1, arg2);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine(string format, params object[] arg) {
            this.Write(format, arg);
            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine() {
            this.Flush();
        }

        /// <inheritdoc />
        public override Task WriteAsync(char value) {
            return Task.Run(() => this.Write(value));
        }

        /// <inheritdoc />
        public override Task WriteAsync(string value) {
            return Task.Run(() => this.Write(value));
        }

        /// <inheritdoc />
        public override Task WriteAsync(char[] buffer, int index, int count) {
            return Task.Run(() => this.Write(buffer, index, count));
        }

        /// <inheritdoc />
        public override Task WriteLineAsync(char value) {
            return Task.Run(() => this.WriteLine(value));
        }

        /// <inheritdoc />
        public override Task WriteLineAsync(string value) {
            return Task.Run(() => this.WriteLine(value));
        }

        /// <inheritdoc />
        public override Task WriteLineAsync(char[] buffer, int index, int count) {
            return Task.Run(() => this.WriteLine(buffer, index, count));
        }

        /// <inheritdoc />
        public override Task WriteLineAsync() {
            return Task.Run(this.WriteLine);
        }

        /// <inheritdoc />
        public override Task FlushAsync() {
            return Task.Run(this.Flush);
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
