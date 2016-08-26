namespace ServiceApi.Server.WebApi.Binding
{
	using System;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http.Controllers;
	using System.Web.Http.Metadata;
	using Microsoft.OData.Core;
	using ServiceApi.Model.Core.Serialization;

	/// <summary>
	/// Parameter binding that binds a ODataQueryOptions instance using the current odata request.
	/// </summary>
	internal class ODataQueryOptionsParameterBinding : HttpParameterBinding
	{
		public ODataQueryOptionsParameterBinding(HttpParameterDescriptor parameterDescriptor)
			: base(parameterDescriptor)
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

			var options = GetQueryOptions(request);
			this.SetValue(actionContext, options);

			return Task.FromResult(0);
		}

		private static QueryOptions GetQueryOptions(HttpRequestMessage request)
		{
			var parser = request.GetODataUriParser();
			try
			{
				var options = new QueryOptions
								{
									SearchClause = parser.ParseSearch(),
									FilterClause = parser.ParseFilter(),
									OrderByClause = parser.ParseOrderBy(),
									SelectExpandClause = parser.ParseSelectAndExpand(),
									Skip = parser.ParseSkip(),
									Top = parser.ParseTop()
								};

				return options;

			}
			catch (Exception)
			{
				// TODO log?
				throw new ODataException("Invalid query string");
			}
		}
	}
}
