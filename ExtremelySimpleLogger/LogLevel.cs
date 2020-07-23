namespace ExtremelySimpleLogger {
    /// <summary>
    /// A log level represents the importance of a message.
    /// The higher the log level (the farther down in the list), the more important it is.
    /// </summary>
    public enum LogLevel {

        /// <summary>
        /// The log level for very high-detail messages that are used for intensive debugging
        /// </summary>
        Trace,
        /// <summary>
        /// The log level for high-detail messages that are used for debugging
        /// </summary>
        Debug,
        /// <summary>
        /// The log level for informational messages
        /// </summary>
        Info,
        /// <summary>
        /// The log level for warnings.
        /// </summary>
        Warn,
        /// <summary>
        /// The log level for errors.
        /// </summary>
        Error,
        /// <summary>
        /// The log level for fatal exceptions, like when the program encounters a crash.
        /// </summary>
        Fatal

    }
}