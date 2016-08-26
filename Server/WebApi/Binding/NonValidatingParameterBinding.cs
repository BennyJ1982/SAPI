namespace ServiceApi.Server.WebApi.Binding
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Net.Http.Formatting;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http.Controllers;
	using System.Web.Http.Metadata;

	/// <summary>
	/// Skips model validation
	/// </summary>
	public sealed class NonValidatingParameterBinding : HttpParameterBinding
	{
		private readonly IEnumerable<MediaTypeFormatter> mediaFormatters;

		public NonValidatingParameterBinding(HttpParameterDescriptor descriptor)
			: base(descriptor)
		{
			var formatters = descriptor.Configuration.Formatters;
			if (formatters == null)
			{
				throw new ArgumentNullException(nameof(formatters));
			}

			this.mediaFormatters = formatters;
		}

		public override bool WillReadBody => true;

		public override Task ExecuteBindingAsync(
			ModelMetadataProvider metadataProvider,
			HttpActionContext actionContext,
			CancellationToken cancellationToken)
		{
			var perRequestFormatters = new List<MediaTypeFormatter>();
			foreach (var formatter in this.mediaFormatters)
			{
				perRequestFormatters.Add(
					formatter.GetPerRequestFormatterInstance(
						this.Descriptor.ParameterType,
						actionContext.Request,
						actionContext.Request.Content.Headers.ContentType));
			}

			var innerBinding = this.CreateInnerBinding(perRequestFormatters);
			Contract.Assert(innerBinding != null);

			return innerBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
		}

		private HttpParameterBinding CreateInnerBinding(IEnumerable<MediaTypeFormatter> perRequestFormatters)
		{
			return this.Descriptor.BindWithFormatter(perRequestFormatters, bodyModelValidator: null);
		}
	}
}