# ExtremelySimpleLogger
**A very simple logger for .NET programs.**

To set up an extremely simple logger, you have to create an instance of the `Logger` class:
```cs
var logger = new Logger {
    Name = "My Logger",
    Sinks = {
        new FileSink("Log.txt", append: true),
        new ConsoleSink()
    }
};
```

Since there are multiple ways for logging data to be processed, the logger needs to receive a set of `Sink` instances. By default, the following sinks are available:
- `FileSink`, which outputs logging data to a file
- `ConsoleSink`, which outputs logging data to the default console

There are multiple ways to easily log messages with your newly created logger:
```cs
// Logging info
logger.Log(LogLevel.Info, "Some information");
logger.Info("Some information, but shorter");

// Logging exceptions
try {
    // some dangerous code
} catch (Exception e) {
    logger.Error("An exception was thrown", e);
}
```