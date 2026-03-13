using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace FluentFtpServer;

/// <summary>
/// A builder for creating instances of <see cref="IFtpClient"/>.
/// </summary>
public class FtpClientBuilder
{
    private readonly FtpOptions _options = new();
    private ILogger<FluentFtpClient> _logger = NullLogger<FluentFtpClient>.Instance;

    /// <summary>
    /// Sets the FTP server address.
    /// </summary>
    /// <param name="server">Host name or IP address.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithServer(string server)
    {
        _options.Server = server;
        return this;
    }

    /// <summary>
    /// Sets the FTP server port.
    /// </summary>
    /// <param name="port">Port number.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithPort(int port)
    {
        _options.Port = port;
        return this;
    }

    /// <summary>
    /// Sets the authentication credentials.
    /// </summary>
    /// <param name="username">FTP username.</param>
    /// <param name="password">FTP password.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithCredentials(string username, string password)
    {
        _options.Username = username;
        _options.Password = password;
        return this;
    }

    /// <summary>
    /// Configures SSL/TLS encryption.
    /// </summary>
    /// <param name="useSsl">True to enable SSL.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithSsl(bool useSsl = false)
    {
        _options.UseSsl = useSsl;
        return this;
    }

    /// <summary>
    /// Sets the connection timeout.
    /// </summary>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithTimeout(int timeoutSeconds)
    {
        _options.TimeoutSeconds = timeoutSeconds;
        return this;
    }

    /// <summary>
    /// Sets the number of retry attempts.
    /// </summary>
    /// <param name="retryAttempts">Retry count.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithRetry(int retryAttempts)
    {
        _options.RetryAttempts = retryAttempts;
        return this;
    }

    /// <summary>
    /// Sets the base path for all operations.
    /// </summary>
    /// <param name="mainPath">The root path on the server.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithMainPath(string mainPath)
    {
        _options.MainPath = mainPath;
        return this;
    }

    /// <summary>
    /// Sets a custom logger for the client.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <returns>The builder instance.</returns>
    public FtpClientBuilder WithLogger(ILogger<FluentFtpClient> logger)
    {
        _logger = logger;
        return this;
    }

    /// <summary>
    /// Builds and returns the <see cref="IFtpClient"/> instance.
    /// </summary>
    /// <returns>A configured <see cref="IFtpClient"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if required parameters are missing.</exception>
    public IFtpClient Build()
    {
        if (string.IsNullOrEmpty(_options.Server) || string.IsNullOrEmpty(_options.Username) || string.IsNullOrEmpty(_options.Password))
            throw new InvalidOperationException("Server, Username and Password cannot be null or empty.");

        var optionsWrapper = Options.Create(_options);
        return new FluentFtpClient(optionsWrapper, _logger);
    }
}
