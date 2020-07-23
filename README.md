# ExtremelySimpleLogger
**A very simple logger for .NET programs.**

To set up an extremely simple logger, add a reference to the [NuGet package](https://www.nuget.org/packages/ExtremelySimpleLogger/) to your project file. Remember to change the `VERSION` to the most recent one.
```xml
<ItemGroup>
    <PackageReference Include="ExtremelySimpleLogger" Version="VERSION" />
</ItemGroup>
```

Next, you have to create an instance of the `Logger` class:
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
- `StringSink`, which stores logging data in a string

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

For more information, you can also check out [the sample](https://github.com/Ellpeck/ExtremelySimpleLogger/blob/master/Sample/Program.cs).