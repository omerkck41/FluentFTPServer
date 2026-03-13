namespace FluentFtpServer;

public sealed class FtpOptions
{
    public const string SectionName = "Ftp";

    public string Server { get; set; } = string.Empty;
    public int Port { get; set; } = 21;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    public int TimeoutSeconds { get; set; } = 10;
    public int RetryAttempts { get; set; } = 3;
    public string MainPath { get; set; } = "/";
}
