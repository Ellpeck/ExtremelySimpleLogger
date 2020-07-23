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
        public string Value => this.builder.ToString();

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="s">The message to log</param>
        public override void Log(string s) {
            this.builder.AppendLine(s);
        }

        /// <summary>
        /// Clears the string that this sink currently contains.
        /// After this call, <see cref="Value"/> will be empty.
        /// </summary>
        public void Clear() {
            this.builder.Clear();
        }

    }
}