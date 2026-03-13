namespace FluentFtpServer;

/// <summary>
/// Defines the core operations for an FTP client.
/// </summary>
public interface IFtpClient : IDisposable
{
    /// <summary>
    /// Asynchronously connects to the FTP server using the configured options.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if connection was successful; otherwise, false.</returns>
    Task<bool> ConnectAsync();

    /// <summary>
    /// Disconnects from the FTP server.
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Checks if a file exists on the FTP server.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    Task<bool> FileExistsAsync(string filePath);

    /// <summary>
    /// Checks if a directory exists on the FTP server.
    /// </summary>
    /// <param name="directoryPath">The path to the directory.</param>
    /// <returns>True if the directory exists; otherwise, false.</returns>
    Task<bool> DirectoryExistsAsync(string directoryPath);

    /// <summary>
    /// Lists all files and directories in the specified path.
    /// </summary>
    /// <param name="directoryPath">The directory path to list. Defaults to root "/".</param>
    /// <returns>A collection of full paths for files and directories.</returns>
    Task<IEnumerable<string>> ListDirectoryAsync(string directoryPath = "/");

    /// <summary>
    /// Creates a directory on the FTP server.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to create.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> CreateDirectoryAsync(string directoryPath);

    /// <summary>
    /// Deletes a directory from the FTP server.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to delete.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> DeleteDirectoryAsync(string directoryPath);

    /// <summary>
    /// Renames a directory or file on the FTP server.
    /// </summary>
    /// <param name="currentDirectoryPath">The current path.</param>
    /// <param name="newDirectoryPath">The new path.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> RenameDirectoryAsync(string currentDirectoryPath, string newDirectoryPath);

    /// <summary>
    /// Uploads a file to the FTP server.
    /// </summary>
    /// <param name="localFilePath">The local path of the file.</param>
    /// <param name="remoteFilePath">The destination path on the FTP server.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> UploadFileAsync(string localFilePath, string remoteFilePath);

    /// <summary>
    /// Uploads a directory and its contents to the FTP server.
    /// </summary>
    /// <param name="localFilePath">The local directory path.</param>
    /// <param name="remoteFilePath">The destination directory path on the FTP server.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> UploadDirectoryAsync(string localFilePath, string remoteFilePath);

    /// <summary>
    /// Downloads a file from the FTP server.
    /// </summary>
    /// <param name="remoteFilePath">The path of the file on the FTP server.</param>
    /// <param name="localFilePath">The local destination path.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> DownloadFileAsync(string remoteFilePath, string localFilePath);

    /// <summary>
    /// Downloads a directory and its contents from the FTP server.
    /// </summary>
    /// <param name="remoteFilePath">The directory path on the FTP server.</param>
    /// <param name="localFilePath">The local destination directory path.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    Task<bool> DownloadDirectoryAsync(string remoteFilePath, string localFilePath);
}
