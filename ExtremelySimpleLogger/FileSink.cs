using System;
using System.IO;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to a file.
    /// </summary>
    public class FileSink : Sink {

        /// <summary>
        /// The <see cref="FileInfo"/> that this sink is currently using as its destination.
        /// </summary>
        public FileInfo CurrentFile {
            get {
                lock (this.file)
                    return this.file;
            }
        }
        
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
            this(new FileInfo(file), append, reopenOnWrite, fileSizeLimit) {}

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

            try {
                var dir = file.Directory;
                if (dir != null && !dir.Exists)
                    dir.Create();
            } catch (Exception e) {
                throw new IOException($"Failed to create directory for file sink {file}", e);
            }

            try {
                if (file.Exists && (!append || file.Length >= fileSizeLimit))
                    file.Delete();
            } catch (Exception e) {
                throw new IOException($"Failed to delete file sink file {file}", e);
            }

            if (!reopenOnWrite) {
                this.writer = this.Append();
                this.writer.AutoFlush = true;
            }
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/> or <see cref="Logger.DefaultFormatter"/>.
        /// </summary>
        /// <param name="logger">The logger that the message was passed to</param>
        /// <param name="level">The importance level of this message</param>
        /// <param name="s">The message to log</param>
        protected override void Log(Logger logger, LogLevel level, string s) {
            lock (this.file) {
                if (this.reopenOnWrite) {
                    using (var w = this.Append())
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

        private StreamWriter Append() {
            try {
                return this.file.AppendText();
            } catch (Exception e) {
                throw new IOException($"Failed to append to file sink {this.file}", e);
            }
        }

    }
}