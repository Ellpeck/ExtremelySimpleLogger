using System.Text;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes output to a string, which can be queried using <see cref="Value"/>.
    /// Note that this uses a <see cref="StringBuilder"/> internally for performance.
    /// </summary>
    public class StringSink : Sink {

        private readonly StringBuilder builder = new StringBuilder();
        /// <summary>
        /// The string that this sink currently contains.
        /// Can be cleared using <see cref="Clear"/>.
        /// </summary>
        public string Value {
            get {
                lock (this.builder)
                    return this.builder.ToString();
            }
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected override void Log(Logger logger, LogLevel level, string s) {
            lock (this.builder)
                this.builder.AppendLine(s);
        }

        /// <summary>
        /// Clears the string that this sink currently contains.
        /// After this call, <see cref="Value"/> will be empty.
        /// </summary>
        public void Clear() {
            lock (this.builder)
                this.builder.Clear();
        }

    }
}