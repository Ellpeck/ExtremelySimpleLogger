using System.IO;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to a file.
    /// </summary>
    public class FileSink : Sink {

        private readonly StreamWriter writer;

        /// <summary>
        /// Creates a new file sink with the given settings.
        /// </summary>
        /// <param name="file">The full, or relative, path of the file to write to</param>
        /// <param name="append">Whether new output should be appended to the old log file</param>
        public FileSink(string file, bool append) :
            this(new FileInfo(file), append) {
        }

        /// <summary>
        /// Creates a new file sink with the given settings.
        /// </summary>
        /// <param name="file">The full, or relative, path of the file to write to</param>
        /// <param name="append">Whether new output should be appended to the old log file</param>
        public FileSink(FileInfo file, bool append) {
            var dir = file.Directory;
            if (dir != null && !dir.Exists)
                dir.Create();

            if (!append && file.Exists)
                file.Delete();

            this.writer = file.AppendText();
            this.writer.AutoFlush = true;
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="s">The message to log</param>
        public override void Log(string s) {
            this.writer.WriteLine(s);
        }

    }
}