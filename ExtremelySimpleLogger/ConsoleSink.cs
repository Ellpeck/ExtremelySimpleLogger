using System;

namespace ExtremelySimpleLogger {
    public class ConsoleSink : Sink {

        public override void Log(string s) {
            Console.WriteLine(s);
        }

    }
}