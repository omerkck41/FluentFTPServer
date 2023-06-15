namespace FTPServer
{
	public class FtpClientFactory
	{
		public IFtpClient CreateFtpClient(string server, int port, string username, string password, bool useSsl, bool useFluentFtp)
		{
			return new FtpClientBuilder()
				.WithServer(server)
				.WithPort(port)
				.WithUsername(username)
				.WithPassword(password)
				.WithSsl(useSsl)
				.Build();
		}
	}
}