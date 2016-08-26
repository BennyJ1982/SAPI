namespace ServiceApi.Server.WebApi
{
	using System;
	using System.Web.Http.Controllers;
	using System.Web.OData.Formatter;
	using ServiceApi.Server.WebApi.Deserialization;
	using ServiceApi.Server.WebApi.Serializers;

	/// <summary>
	/// Enables a controller to use a custom odata serializer and deserializer which supports the service API odata entity interfaces.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class EnableServiceApiFormattersAttribute : Attribute, IControllerConfiguration
	{
		public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
		{
			var odataFormatters = ODataMediaTypeFormatters.Create(new SerializerProvider(), new DerializerProvider());
			controllerSettings.Formatters.InsertRange(0, odataFormatters);
		}    
	}
}
