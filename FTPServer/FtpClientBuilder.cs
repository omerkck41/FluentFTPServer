using FluentFTP;

namespace FTPServer
{
	public class FtpClientBuilder
	{
		private string? _server;
		private int _port;
		private string? _username;
		private string? _password;
		private bool _useSsl;
		private int _timeout;
		private int _retry;

		public FtpClientBuilder WithServer(string server)
		{
			_server = server;
			return this;
		}
		public FtpClientBuilder WithPort(int port)
		{
			_port = port;
			return this;
		}
		public FtpClientBuilder WithUsername(string username)
		{
			_username = username;
			return this;
		}
		public FtpClientBuilder WithPassword(string password)
		{
			_password = password;
			return this;
		}
		public FtpClientBuilder WithCredentials(string username, string password)
		{
			_username = username;
			_password = password;
			return this;
		}
        public FtpClientBuilder WithSsl(bool useSsl = false)
		{
			_useSsl = useSsl;
			return this;
		}
		public FtpClientBuilder WithTimeout(int timeout)
		{
			_timeout = timeout;
			return this;
		}
		public FtpClientBuilder WithRetry(int retry)
		{
			_retry = retry;
			return this;
		}


		public IFtpClient Build()
		{
			if (string.IsNullOrEmpty(_server) || string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
				throw new Exception("Cannot be null or empty");

			var ftpClient = new AsyncFtpClient();
			ftpClient.Config.ConnectTimeout = 1000 * (_timeout > 0 ? _timeout : 10);
			ftpClient.Config.RetryAttempts = _retry;

			return new FluentFtpClient(ftpClient, _server, _port, _username, _password, _useSsl);
		}
	}
}