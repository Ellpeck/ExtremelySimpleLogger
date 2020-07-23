using System;
using System.IO;

namespace ExtremelySimpleLogger {
    public class FileSink : Sink {

        private readonly StreamWriter writer;

        public FileSink(string file, bool append) :
            this(new FileInfo(file), append) {
        }

        public FileSink(FileInfo file, bool append) {
            var dir = file.Directory;
            if (dir != null && !dir.Exists)
                dir.Create();

            if (!append && file.Exists)
                file.Delete();

            this.writer = file.AppendText();
            this.writer.AutoFlush = true;
        }

        public override void Log(string s) {
            this.writer.WriteLine(s);
        }

    }
}