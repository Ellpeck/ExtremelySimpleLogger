using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A <see cref="Sink"/> that writes log output to a set of directories.
    /// This sink differs from <see cref="FileSink"/> in that it manages multiple log files in a directory, where a new file will be created every time the sink is created.
    /// Additionally, this sink will automatically delete the oldest log files if the amount of files exceeds a set limit.
    /// </summary>
    public class DirectorySink : Sink {

        /// <summary>
        /// The set of old files that are currently in the directory that this sink is referencing, and that have not been deleted on construction.
        /// The files in this list are ordered by creation date in ascending order, meaning that the first entry is the least recently created one.
        /// Note that this collection does not contain the <see cref="CurrentFile"/>.
        /// </summary>
        public readonly IList<FileInfo> OldFiles;
        /// <summary>
        /// The <see cref="DirectoryInfo"/> that this sink is currently using as its destination to store the <see cref="OldFiles"/> and <see cref="CurrentFile"/>.
        /// </summary>
        public readonly DirectoryInfo Directory;
        /// <summary>
        /// The <see cref="FileInfo"/> that this sink is currently using as its destination.
        /// </summary>
        public FileInfo CurrentFile {
            get {
                lock (this.file)
                    return this.file;
            }
        }

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
            this(new DirectoryInfo(directory), maxFiles, reopenOnWrite, dateFormat) {}

        /// <summary>
        /// Creates a new directory sink with the given settings.
        /// </summary>
        /// <param name="directory">The directory that this sink should operate in</param>
        /// <param name="maxFiles">The maximum amount of files that can exist in the directory before the oldest one gets deleted. 10 by default.</param>
        /// <param name="reopenOnWrite">Whether this sink should reopen the file every time it logs to it. If this is false, the file will be kept open by this sink.</param>
        /// <param name="dateFormat">The way the name of the current log file gets formatted. <code>yy-MM-dd_HH-mm-ss</code> by default.</param>
        public DirectorySink(DirectoryInfo directory, int maxFiles = 10, bool reopenOnWrite = false, string dateFormat = DefaultDateFormat) {
            this.reopenOnWrite = reopenOnWrite;
            this.Directory = directory;

            try {
                if (!directory.Exists)
                    directory.Create();
            } catch (Exception e) {
                throw new IOException($"Failed to create directory sink directory {directory}", e);
            }

            try {
                // delete files in order of creation time so that older files are deleted first
                var ordered = directory.EnumerateFiles().OrderBy(f => f.CreationTime).ToList();
                while (ordered.Count >= maxFiles) {
                    ordered[0].Delete();
                    ordered.RemoveAt(0);
                }
                this.OldFiles = ordered.AsReadOnly();
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
                throw new IOException($"Failed to append to directory sink file {this.file}", e);
            }
        }

    }
}