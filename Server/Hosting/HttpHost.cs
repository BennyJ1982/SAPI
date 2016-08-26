namespace ServiceApi.Server.Hosting
{
	using System;
	using System.Web.Http;
	using Microsoft.Owin.Hosting;
	using Microsoft.Owin.Logging;
	using Owin;
	using ServiceApi.Server.Middleware;

	public class HttpHost : IHttpHost
	{
		private readonly HttpConfiguration httpConfiguration;

		private IDisposable webApp;

		public HttpHost(HttpConfiguration httpConfiguration)
		{
			this.httpConfiguration = httpConfiguration;
		}

		public void Open(string baseUrl)
		{
			var startOptions = new StartOptions(baseUrl);
			this.webApp = WebApp.Start(startOptions, this.ConfigurePipeline);
		}

		// Dispose() calls Dispose(true)
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void ConfigurePipeline(IAppBuilder appBuilder)
		{
			// logging
			appBuilder.SetLoggerFactory(new ConsoleLoggerFactory());
			appBuilder.UseLogging();

			// use Web API
			appBuilder.UseWebApi(this.httpConfiguration);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				if (this.webApp != null)
				{
					this.webApp.Dispose();
					this.webApp = null;
				}
			}
		}
	}
}
