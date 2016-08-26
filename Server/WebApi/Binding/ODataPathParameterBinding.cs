namespace ServiceApi.Server.WebApi.Binding
{
	using System;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http.Controllers;
	using System.Web.Http.Metadata;
	using Microsoft.OData.Core;
	using Microsoft.OData.Core.UriParser.Semantic;

	/// <summary>
	/// Parameter binding for parameters using ODataPath from Microsoft.OData.Core. 
	/// Note: WebApi has its own ODataPath implementation which can be bound out-of-the-box but we don't want to use that one.
	/// </summary>
	internal class ODataPathParameterBinding : HttpParameterBinding
	{
		public ODataPathParameterBinding(HttpParameterDescriptor descriptor)
			: base(descriptor)
		{
		}

		public override Task ExecuteBindingAsync(
			ModelMetadataProvider metadataProvider,
			HttpActionContext actionContext,
			CancellationToken cancellationToken)
		{
			if (actionContext == null)
			{
				throw new ArgumentException("actionContext");
			}

			var request = actionContext.Request;
			if (request == null)
			{
				throw new ArgumentException("actionContext.request");
			}


			var options = GetODataPath(request);
			this.SetValue(actionContext, options);

			return Task.FromResult(0);
		}

		private static ODataPath GetODataPath(HttpRequestMessage request)
		{
			var parser = request.GetODataUriParser();
			try
			{
				var path = parser.ParsePath();
				return path;

			}
			catch (Exception)
			{
				// TODO log?
				throw new ODataException("Invalid odata path");
			}
		}
	}
}
