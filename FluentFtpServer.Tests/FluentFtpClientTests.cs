using FluentFtpServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FluentFtpServer.Tests;

public class FluentFtpClientTests
{
    private readonly Mock<ILogger<FluentFtpClient>> _loggerMock;
    private readonly IOptions<FtpOptions> _options;

    public FluentFtpClientTests()
    {
        _loggerMock = new Mock<ILogger<FluentFtpClient>>();
        _options = Options.Create(new FtpOptions
        {
            Server = "localhost",
            Username = "test",
            Password = "password",
            MainPath = "/test-root/"
        });
    }

    [Fact]
    public void Options_Should_Be_Configured_Correctly()
    {
        // Arrange & Act
        var client = new FluentFtpClient(_options, _loggerMock.Object);

        // Assert
        Assert.NotNull(client);
    }
}
