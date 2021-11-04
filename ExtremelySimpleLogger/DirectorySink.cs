using System;
using System.IO;
using System.Linq;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to a set of directories.
    /// This sink differs from <see cref="FileSink"/> in that it manages multiple log files in a directory, where a new file will be created every time the sink is created.
    /// Additionally, this sink will automatically delete the oldest log files if the amount of files exceeds a set limit.
    /// </summary>
    public class DirectorySink : Sink {

        private const string DefaultDateFormat = "yy-MM-dd_HH-mm-ss";
        private readonly FileInfo file;
        private readonly StreamWriter writer;
        private readonly bool reopenOnWrite;

        /// <summary>
        /// Creates a new directory sink with the given settings.
        /// </summary>
        /// <param name="directory">The directory that this sink should operate in</param>
        /// <param name="maxFiles">The maximum amount of files that can exist in the directory before the oldest one gets deleted. 10 by default.</param>
        /// <param name="reopenOnWrite">Whether this sink should reopen the file every time it logs to it. If this is false, the file will be kept open by this sink.</param>
        /// <param name="dateFormat">The way the name of the current log file gets formatted. <code>yy-MM-dd_HH-mm-ss</code> by default.</param>
        public DirectorySink(string directory, int maxFiles = 10, bool reopenOnWrite = false, string dateFormat = DefaultDateFormat) :
            this(new DirectoryInfo(directory), maxFiles, reopenOnWrite, dateFormat) {
        }

        /// <summary>
        /// Creates a new directory sink with the given settings.
        /// </summary>
        /// <param name="directory">The directory that this sink should operate in</param>
        /// <param name="maxFiles">The maximum amount of files that can exist in the directory before the oldest one gets deleted. 10 by default.</param>
        /// <param name="reopenOnWrite">Whether this sink should reopen the file every time it logs to it. If this is false, the file will be kept open by this sink.</param>
        /// <param name="dateFormat">The way the name of the current log file gets formatted. <code>yy-MM-dd_HH-mm-ss</code> by default.</param>
        public DirectorySink(DirectoryInfo directory, int maxFiles = 10, bool reopenOnWrite = false, string dateFormat = DefaultDateFormat) {
            this.reopenOnWrite = reopenOnWrite;

            try {
                if (!directory.Exists)
                    directory.Create();
            } catch (Exception e) {
                throw new IOException($"Failed to create directory sink directory {directory}", e);
            }

            try {
                // delete old files
                var files = directory.GetFiles();
                if (files.Length >= maxFiles) {
                    // order files by their creation time so that older files are deleted first
                    var ordered = files.OrderBy(f => f.CreationTime).ToList();
                    while (ordered.Count >= maxFiles) {
                        ordered[0].Delete();
                        ordered.RemoveAt(0);
                    }
                }
            } catch (Exception e) {
                throw new IOException($"Failed to delete old files in directory sink {directory}", e);
            }

            var date = DateTime.Now.ToString(dateFormat);
            this.file = new FileInfo(Path.Combine(directory.FullName, $"{date}.txt"));
            if (!reopenOnWrite) {
                this.writer = this.Append();
                this.writer.AutoFlush = true;
            }
        }

        /// <summary>
        /// Logs the given message, which has already been formatted using <see cref="Sink.Formatter"/>.
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
                throw new IOException($"Failed to append to directory sink file {this.file}", e);
            }
        }

    }
}