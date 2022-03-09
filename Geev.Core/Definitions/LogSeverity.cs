namespace Geev.Core.Definitions;

/// <summary>
/// Indicates severity for a log. 
/// Basically it corresnponds to log level but we introduced a new type to avoid dependency on a specific platform.
/// </summary>
public enum LogSeverity
{
    /// <summary>
    /// Debug.
    /// </summary>
    Debug,

    /// <summary>
    /// Info.
    /// </summary>
    Info,

    /// <summary>
    /// Warn.
    /// </summary>
    Warn,

    /// <summary>
    /// Error.
    /// </summary>
    Error,

    /// <summary>
    /// Critical.
    /// </summary>
    Critical
}
