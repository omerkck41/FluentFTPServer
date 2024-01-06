using FluentFTP;
using System.Net;

namespace FTPServer
{
	public class FluentFtpClient : IFtpClient
	{
		private readonly AsyncFtpClient _asyncFtpClient;
		private readonly string _server;
		private readonly int _port;
		private readonly string _username;
		private readonly string _password;
		private readonly bool _useSsl;
		private bool _disposed;
		private readonly string _mainpath;

		public FluentFtpClient(AsyncFtpClient asyncftpClient, string server, int port, string username, string password, bool useSsl, string mainpath)
{
    _server = server;
    _port = port;
    _username = username;
    _password = password;
    _useSsl = useSsl;
    _mainpath = mainpath;

    _asyncFtpClient = asyncftpClient;
    _asyncFtpClient.Config.EncryptionMode = _useSsl ? FtpEncryptionMode.Explicit : FtpEncryptionMode.None;
    _asyncFtpClient.ValidateCertificate += (control, e) => e.Accept = true;
    _asyncFtpClient.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;

}

		public async Task<bool> ConnectAsync()
		{
			try
			{
				if(_asyncFtpClient.IsConnected) return true;

				var token = new CancellationToken();

				_asyncFtpClient.Host = _server;
				_asyncFtpClient.Credentials = new NetworkCredential(_username, _password);
				_asyncFtpClient.Port = _port;

				await _asyncFtpClient.Connect(token);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public void Disconnect()
		{
			if (_asyncFtpClient.IsConnected)
			{
				_asyncFtpClient.Disconnect();
			}
		}
		
		public async Task<bool> FileExistsAsync(string filePath)
{
    try
    {
        await ConnectAsync();
        var res = await _asyncFtpClient.FileExists(_mainpath + IndexPathControl(filePath));

        return res;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> DirectoryExistsAsync(string directoryPath)
{
    try
    {
        await ConnectAsync();
        var res = await _asyncFtpClient.DirectoryExists(_mainpath + IndexPathControl(directoryPath));

        return res;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> CreateDirectoryAsync(string directoryPath)
{
    try
    {
        await ConnectAsync();
        var res = await _asyncFtpClient.CreateDirectory(_mainpath + IndexPathControl(directoryPath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> RenameDirectoryAsync(string currentDirectoryPath, string newDirectoryPath)
{
    try
    {
        await ConnectAsync();
        await _asyncFtpClient.Rename(_mainpath + IndexPathControl(currentDirectoryPath), _mainpath + IndexPathControl(newDirectoryPath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> DeleteDirectoryAsync(string directoryPath)
{
    try
    {
        await ConnectAsync();
        await _asyncFtpClient.DeleteDirectory(_mainpath + IndexPathControl(directoryPath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<IEnumerable<string>> ListDirectoryAsync(string directoryPath = "/")
{
    try
    {
        await ConnectAsync();

        var listing = await _asyncFtpClient.GetListing(_mainpath + IndexPathControl(directoryPath));

        var directories = listing.Where(x => x.Type == FtpObjectType.Directory).Select(x => x.FullName);
        var files = listing.Where(x => x.Type == FtpObjectType.File).Select(x => x.FullName);

        return directories.Concat(files);
    }
    catch (Exception)
    {
        return Enumerable.Empty<string>();
    }
}
public async Task<bool> DownloadFileAsync(string remoteFilePath, string localFilePath)
{
    try
    {
        await ConnectAsync();

        using var fileStream = File.OpenWrite(localFilePath);
        await _asyncFtpClient.DownloadStream(fileStream, _mainpath + IndexPathControl(remoteFilePath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> DownloadDirectoryAsync(string remoteFilePath, string localFilePath)
{
    try
    {
        await ConnectAsync();
        await _asyncFtpClient.DownloadDirectory(localFilePath, _mainpath + IndexPathControl(remoteFilePath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> UploadFileAsync(string localFilePath, string remoteFilePath)
{
    try
    {
        await ConnectAsync();

        using var fileStream = File.OpenRead(localFilePath);
        await _asyncFtpClient.UploadStream(fileStream, _mainpath + IndexPathControl(remoteFilePath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}
public async Task<bool> UploadDirectoryAsync(string localFilePath, string remoteFilePath)
{
    try
    {
        await ConnectAsync();
        await _asyncFtpClient.UploadDirectory(localFilePath, _mainpath + IndexPathControl(remoteFilePath));

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}

        static string IndexPathControl(string path)
        {
            // Check if there is a leading / and remove it
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            return path;
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
					// Dispose managed resources
					_asyncFtpClient.Dispose();
				}

				// Dispose unmanaged resources
				Disconnect();
				_disposed = true;
			}
		}
	}
}