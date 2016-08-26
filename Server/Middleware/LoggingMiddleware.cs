namespace ServiceApi.Server.Middleware
{
	using System.Threading.Tasks;
	using Microsoft.Owin;
	using Microsoft.Owin.Logging;
	using Owin;

	public class LoggingMiddleware : OwinMiddleware
	{
		private readonly ILogger logger;

		public LoggingMiddleware(OwinMiddleware next, IAppBuilder app)
			: base(next)
		{
			this.logger = app.CreateLogger<LoggingMiddleware>();
		}

		public override Task Invoke(IOwinContext context)
		{
			this.logger.WriteVerbose($"{context.Request.Scheme} {context.Request.Method}: {context.Request.Path}{context.Request.QueryString}");

			if (this.Next != null)
			{
				return this.Next.Invoke(context);
			}
			else
			{
				context.Response.Headers.Add(
						"Content-Type", new[] { "text/plain" });

				return context.Response.WriteAsync("Logging sample is runnig!");
			}
		}
	}
}
