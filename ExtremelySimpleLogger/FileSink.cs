using System.IO;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to a file.
    /// </summary>
    public class FileSink : Sink {

        private const int OneGb = 1024 * 1024 * 1024;
        private readonly FileInfo file;
        private readonly StreamWriter writer;
        private readonly bool reopenOnWrite;

        /// <summary>
        /// Creates a new file sink with the given settings.
        /// </summary>
        /// <param name="file">The full, or relative, path of the file to write to</param>
        /// <param name="append">Whether new output should be appended to the old log file</param>
        /// <param name="reopenOnWrite">Whether this file sink should reopen the file every time it logs to it. If this is false, the file will be kept open by this sink.</param>
        /// <param name="fileSizeLimit">If <paramref name="append"/> is true, this property determines how big the log file has to be (in bytes) before it is deleted on startup. Defaults to 1gb.</param>
        public FileSink(string file, bool append, bool reopenOnWrite = false, int fileSizeLimit = OneGb) :
            this(new FileInfo(file), append, reopenOnWrite, fileSizeLimit) {
        }

        /// <summary>
        /// Creates a new file sink with the given settings.
        /// </summary>
        /// <param name="file">The full, or relative, path of the file to write to</param>
        /// <param name="append">Whether new output should be appended to the old log file</param>
        /// <param name="reopenOnWrite">Whether this file sink should reopen the file every time it logs to it. If this is false, the file will be kept open by this sink.</param>
        /// <param name="fileSizeLimit">If <paramref name="append"/> is true, this property determines how big the log file has to be (in bytes) before it is deleted on startup.</param>
        public FileSink(FileInfo file, bool append, bool reopenOnWrite = false, int fileSizeLimit = OneGb) {
            this.reopenOnWrite = reopenOnWrite;
            this.file = file;

            var dir = file.Directory;
            if (dir != null && !dir.Exists)
                dir.Create();

            if (file.Exists && (!append || file.Length >= fileSizeLimit))
                file.Delete();

            if (!reopenOnWrite) {
                this.writer = file.AppendText();
                this.writer.AutoFlush = true;
            }
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
        /// </summary>
        /// <param name="s">The message to log</param>
        public override void Log(string s) {
            lock (this.file) {
                if (this.reopenOnWrite) {
                    using (var w = this.file.AppendText())
                        w.WriteLine(s);
                } else {
                    this.writer.WriteLine(s);
                }
            }
        }

        /// <summary>
        /// Disposes this sink, freeing all of the resources it uses.
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            lock (this.file) {
                if (!this.reopenOnWrite)
                    this.writer.Dispose();
            }
        }

    }
}