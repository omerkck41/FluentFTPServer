using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace FluentFtpServer;

public class FtpClientBuilder
{
    private readonly FtpOptions _options = new();
    private ILogger<FluentFtpClient> _logger = NullLogger<FluentFtpClient>.Instance;

    public FtpClientBuilder WithServer(string server)
    {
        _options.Server = server;
        return this;
    }

    public FtpClientBuilder WithPort(int port)
    {
        _options.Port = port;
        return this;
    }

    public FtpClientBuilder WithCredentials(string username, string password)
    {
        _options.Username = username;
        _options.Password = password;
        return this;
    }

    public FtpClientBuilder WithSsl(bool useSsl = false)
    {
        _options.UseSsl = useSsl;
        return this;
    }

    public FtpClientBuilder WithTimeout(int timeoutSeconds)
    {
        _options.TimeoutSeconds = timeoutSeconds;
        return this;
    }

    public FtpClientBuilder WithRetry(int retryAttempts)
    {
        _options.RetryAttempts = retryAttempts;
        return this;
    }

    public FtpClientBuilder WithMainPath(string mainPath)
    {
        _options.MainPath = mainPath;
        return this;
    }

    public FtpClientBuilder WithLogger(ILogger<FluentFtpClient> logger)
    {
        _logger = logger;
        return this;
    }

    public IFtpClient Build()
    {
        if (string.IsNullOrEmpty(_options.Server) || string.IsNullOrEmpty(_options.Username) || string.IsNullOrEmpty(_options.Password))
            throw new InvalidOperationException("Server, Username and Password cannot be null or empty.");

        var optionsWrapper = Options.Create(_options);
        return new FluentFtpClient(optionsWrapper, _logger);
    }
}
