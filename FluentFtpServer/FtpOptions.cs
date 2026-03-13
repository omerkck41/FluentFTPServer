namespace FluentFtpServer;

/// <summary>
/// Represents configuration options for the FTP client.
/// </summary>
public sealed class FtpOptions
{
    /// <summary>
    /// The name of the configuration section.
    /// </summary>
    public const string SectionName = "Ftp";

    /// <summary>
    /// The FTP server hostname or IP address.
    /// </summary>
    public string Server { get; set; } = string.Empty;

    /// <summary>
    /// The FTP server port. Defaults to 21.
    /// </summary>
    public int Port { get; set; } = 21;

    /// <summary>
    /// The username for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether to use SSL/TLS for secure communication.
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Connection timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 10;

    /// <summary>
    /// Number of retry attempts for failed operations.
    /// </summary>
    public int RetryAttempts { get; set; } = 3;

    /// <summary>
    /// The default base path for FTP operations.
    /// </summary>
    public string MainPath { get; set; } = "/";
}
