namespace MovieService.Core.Logging
{
    /// <summary>
    /// Specifies verbosity when flushing an object to log.
    /// Object is flushed to log, if verbosity of log is greater or equal to object's verbosity.
    /// </summary>
    public enum Verbosity
    {
        Default = 0,
        Full = 1,
        Empty = 2
    }
}
