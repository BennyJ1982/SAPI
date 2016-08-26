namespace ServiceApi.Server.WebApi.Binding
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Net.Http.Headers;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http.Controllers;
	using System.Web.OData.Formatter;

	/// <summary>
	/// Attribute for intercepting controler settings creation and media formatter selection
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ControllerDebugAttribute : Attribute, IControllerConfiguration
	{
		/// <summary>
		/// Callback invoked to set per-controller overrides for this controllerDescriptor.
		/// </summary>
		/// <param name="controllerSettings">The controller settings to initialize.</param>
		/// <param name="controllerDescriptor">The controller descriptor. Note that the <see
		/// cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> can be associated with the derived
		/// controller type given that <see cref="T:System.Web.Http.Controllers.IControllerConfiguration" /> is
		/// inherited.</param>
		public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
		{
			var formatters = controllerSettings.Formatters.OfType<ODataMediaTypeFormatter>().ToArray();
			foreach (var formatter in formatters)
			{
				controllerSettings.Formatters.Remove(formatter);
				controllerSettings.Formatters.Add(new MediaTypeFormatterWrapper(formatter));
			}
		}

		/// <summary>
		/// Wrapper for all media type formatters. Use this for debugging purposes when intercepting media formatting.
		/// </summary>
		private class MediaTypeFormatterWrapper : MediaTypeFormatter
		{
			private readonly MediaTypeFormatter innerFormatter;

			public MediaTypeFormatterWrapper(MediaTypeFormatter innerFormatter)
				: base(innerFormatter)
			{
				this.innerFormatter = innerFormatter;
			}

			public override bool CanReadType(Type type)
			{
				return this.innerFormatter.CanReadType(type);
			}

			public override bool CanWriteType(Type type)
			{
				return this.innerFormatter.CanWriteType(type);
			}

			public override MediaTypeFormatter GetPerRequestFormatterInstance(
				Type type,
				HttpRequestMessage request,
				MediaTypeHeaderValue mediaType)
			{
				var formatter = this.innerFormatter.GetPerRequestFormatterInstance(type, request, mediaType);
				return formatter != null ? new MediaTypeFormatterWrapper(formatter) : null;
			}

			public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
			{
				this.innerFormatter.SetDefaultContentHeaders(type, headers, mediaType);
			}

			public override Task<object> ReadFromStreamAsync(
				Type type,
				Stream readStream,
				HttpContent content,
				IFormatterLogger formatterLogger)
			{
				return this.innerFormatter.ReadFromStreamAsync(type, readStream, content, formatterLogger);
			}

			public override Task WriteToStreamAsync(
				Type type,
				object value,
				Stream writeStream,
				HttpContent content,
				TransportContext transportContext,
				CancellationToken cancellationToken)
			{
				return this.innerFormatter.WriteToStreamAsync(type, value, writeStream, content, transportContext, cancellationToken);
			}
		}
	}
  
}
