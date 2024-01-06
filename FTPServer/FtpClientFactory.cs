namespace FTPServer
{
	public class FtpClientFactory
	{
		public IFtpClient CreateFtpClient(string server, int port, string username, string password, bool useSsl, string mainpath, bool useFluentFtp)
		{
			return new FtpClientBuilder()
				.WithServer(server)
				.WithPort(port)
				.WithCredentials(username, password)
				.WithSsl(useSsl)
				.WithMainPath(mainpath)
				.Build();
		}
	}
}