namespace ServiceApi.Server.WebApi.Routing
{
	using System;
	using System.Diagnostics;
	using System.Web.OData.Routing;
	using Microsoft.OData.Edm;
	using ServiceApi.Model.Core.Serialization;

	public class LoggingPathHandler : DefaultODataPathHandler
	{
		private readonly IMappingLogger logger;

		public LoggingPathHandler(IMappingLogger logger)
		{
			this.logger = logger;
		}

		public override ODataPath Parse(IEdmModel model, string serviceRoot, string odataPath)
		{
			try
			{
				return base.Parse(model, serviceRoot, odataPath);
			}
			catch (Exception exception)
			{
				this.logger.Write(TraceEventType.Critical, exception.Message);
				throw;
			}
		}
	}
}
