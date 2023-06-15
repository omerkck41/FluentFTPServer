namespace FTPServer
{
	public interface IFtpClient : IDisposable
	{
		Task<bool> ConnectAsync();
		void Disconnect();
		Task<bool> FileExistsAsync(string filePath);
		Task<bool> DirectoryExistsAsync(string directoryPath);
		Task<IEnumerable<string>> ListDirectoryAsync(string directoryPath);
		Task<bool> CreateDirectoryAsync(string directoryPath);
		Task<bool> DeleteDirectoryAsync(string directoryPath);
		Task<bool> RenameDirectoryAsync(string currentDirectoryPath, string newDirectoryPath);
		Task<bool> UploadFileAsync(string localFilePath, string remoteFilePath);
		Task<bool> UploadDirectoryAsync(string localFilePath, string remoteFilePath);
		Task<bool> DownloadFileAsync(string remoteFilePath, string localFilePath);
		Task<bool> DownloadDirectoryAsync(string remoteFilePath, string localFilePath);
	}
}