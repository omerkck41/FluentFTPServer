using FluentFTP;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace FluentFtpServer;

public class FluentFtpClient : IFtpClient
{
    private readonly AsyncFtpClient _asyncFtpClient;
    private readonly FtpOptions _options;
    private readonly ILogger<FluentFtpClient> _logger;
    private bool _disposed;

    public FluentFtpClient(IOptions<FtpOptions> options, ILogger<FluentFtpClient> logger)
    {
        _options = options.Value;
        _logger = logger;

        _asyncFtpClient = new AsyncFtpClient
        {
            Host = _options.Server,
            Port = _options.Port,
            Credentials = new NetworkCredential(_options.Username, _options.Password),
            Config =
            {
                ConnectTimeout = 1000 * (_options.TimeoutSeconds > 0 ? _options.TimeoutSeconds : 10),
                RetryAttempts = _options.RetryAttempts,
                EncryptionMode = _options.UseSsl ? FtpEncryptionMode.Explicit : FtpEncryptionMode.None,
                DataConnectionType = FtpDataConnectionType.AutoPassive
            }
        };

        _asyncFtpClient.ValidateCertificate += (_, e) => e.Accept = true;
    }

    public async Task<bool> ConnectAsync()
    {
        try
        {
            if (_asyncFtpClient.IsConnected) return true;
            await _asyncFtpClient.Connect();
            _logger.LogInformation("Connected to FTP server {Server}", _options.Server);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to FTP server {Server}", _options.Server);
            return false;
        }
    }

    public void Disconnect()
    {
        if (_asyncFtpClient.IsConnected)
        {
            _asyncFtpClient.Disconnect();
            _logger.LogInformation("Disconnected from FTP server {Server}", _options.Server);
        }
    }

    public async Task<bool> FileExistsAsync(string filePath)
    {
        try
        {
            await ConnectAsync();
            var path = BuildPath(filePath);
            var exists = await _asyncFtpClient.FileExists(path);
            _logger.LogDebug("File {Path} existence check: {Exists}", path, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if file exists: {Path}", filePath);
            return false;
        }
    }

    public async Task<bool> DirectoryExistsAsync(string directoryPath)
    {
        try
        {
            await ConnectAsync();
            var path = BuildPath(directoryPath);
            return await _asyncFtpClient.DirectoryExists(path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if directory exists: {Path}", directoryPath);
            return false;
        }
    }

    public async Task<bool> CreateDirectoryAsync(string directoryPath)
    {
        try
        {
            await ConnectAsync();
            var path = BuildPath(directoryPath);
            await _asyncFtpClient.CreateDirectory(path);
            _logger.LogInformation("Directory created: {Path}", path);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating directory: {Path}", directoryPath);
            return false;
        }
    }

    public async Task<bool> RenameDirectoryAsync(string currentDirectoryPath, string newDirectoryPath)
    {
        try
        {
            await ConnectAsync();
            var oldPath = BuildPath(currentDirectoryPath);
            var newPath = BuildPath(newDirectoryPath);
            await _asyncFtpClient.Rename(oldPath, newPath);
            _logger.LogInformation("Renamed {OldPath} to {NewPath}", oldPath, newPath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renaming directory from {OldPath} to {NewPath}", currentDirectoryPath, newDirectoryPath);
            return false;
        }
    }

    public async Task<bool> DeleteDirectoryAsync(string directoryPath)
    {
        try
        {
            await ConnectAsync();
            var path = BuildPath(directoryPath);
            await _asyncFtpClient.DeleteDirectory(path);
            _logger.LogInformation("Directory deleted: {Path}", path);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting directory: {Path}", directoryPath);
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListDirectoryAsync(string directoryPath = "/")
    {
        try
        {
            await ConnectAsync();
            var path = BuildPath(directoryPath);
            var listing = await _asyncFtpClient.GetListing(path);
            return listing.Select(x => x.FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing directory: {Path}", directoryPath);
            return Enumerable.Empty<string>();
        }
    }

    public async Task<bool> DownloadFileAsync(string remoteFilePath, string localFilePath)
    {
        try
        {
            await ConnectAsync();
            var remotePath = BuildPath(remoteFilePath);
            using var fileStream = File.OpenWrite(localFilePath);
            await _asyncFtpClient.DownloadStream(fileStream, remotePath);
            _logger.LogInformation("Downloaded {RemotePath} to {LocalPath}", remotePath, localFilePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file from {RemotePath} to {LocalPath}", remoteFilePath, localFilePath);
            return false;
        }
    }

    public async Task<bool> DownloadDirectoryAsync(string remoteFilePath, string localFilePath)
    {
        try
        {
            await ConnectAsync();
            var remotePath = BuildPath(remoteFilePath);
            await _asyncFtpClient.DownloadDirectory(localFilePath, remotePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading directory from {RemotePath} to {LocalPath}", remoteFilePath, localFilePath);
            return false;
        }
    }

    public async Task<bool> UploadFileAsync(string localFilePath, string remoteFilePath)
    {
        try
        {
            await ConnectAsync();
            var remotePath = BuildPath(remoteFilePath);
            using var fileStream = File.OpenRead(localFilePath);
            await _asyncFtpClient.UploadStream(fileStream, remotePath);
            _logger.LogInformation("Uploaded {LocalPath} to {RemotePath}", localFilePath, remotePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file from {LocalPath} to {RemotePath}", localFilePath, remoteFilePath);
            return false;
        }
    }

    public async Task<bool> UploadDirectoryAsync(string localFilePath, string remoteFilePath)
    {
        try
        {
            await ConnectAsync();
            var remotePath = BuildPath(remoteFilePath);
            await _asyncFtpClient.UploadDirectory(localFilePath, remotePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading directory from {LocalPath} to {RemotePath}", localFilePath, remoteFilePath);
            return false;
        }
    }

    private string BuildPath(string path)
    {
        var mainPath = _options.MainPath.EndsWith('/') ? _options.MainPath : $"{_options.MainPath}/";
        var subPath = path.StartsWith('/') ? path[1..] : path;
        return $"{mainPath}{subPath}";
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _asyncFtpClient.Dispose();
            }
            _disposed = true;
        }
    }
}
