using System.IO;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to an underlying <see cref="TextWriter"/>.
    /// Note that <see cref="ConsoleSink"/> is a variation of this sink that additionally includes console colors.
    /// </summary>
    public class WriterSink : Sink {

        private readonly TextWriter writer;
        private readonly bool autoClose;

        /// <summary>
        /// Creates a new writer sink with the given settings.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="autoClose">Whether the underlying <paramref name="writer"/> should be closed automatically when this sink is disposed in <see cref="Dispose"/>.</param>
        public WriterSink(TextWriter writer, bool autoClose = false) {
            this.writer = writer;
            this.autoClose = autoClose;
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/> or <see cref="Logger.DefaultFormatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected override void Log(Logger logger, LogLevel level, string s) {
            lock (this.writer)
                this.writer.WriteLine(s);
        }

        /// <inheritdoc />
        public override void Dispose() {
            if (this.autoClose) {
                lock (this.writer)
                    this.writer.Dispose();
            }
        }

    }
}
