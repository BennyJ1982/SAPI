namespace ServiceApi.Server.Middleware
{
	using Microsoft.Owin.Logging;

	public class ConsoleLoggerFactory : ILoggerFactory
	{
		public ILogger Create(string name)
		{
			return new ConsoleLogger();
		}
	}
}
