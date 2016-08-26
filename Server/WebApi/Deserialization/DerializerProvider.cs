namespace ServiceApi.Server.WebApi.Deserialization
{
	using System.Web.OData.Formatter.Deserialization;
	using Microsoft.OData.Edm;

	public class DerializerProvider : DefaultODataDeserializerProvider
	{
		private readonly ODataEntityDeserializer referenceLinkParsingDeserializer;

		public DerializerProvider()
		{
			this.referenceLinkParsingDeserializer = new ReferenceLinkParsingEntityDeserializer(this);
		}

		public override ODataEdmTypeDeserializer GetEdmTypeDeserializer(IEdmTypeReference edmType)
		{
			return edmType.IsEntity() ? this.referenceLinkParsingDeserializer : base.GetEdmTypeDeserializer(edmType);
		}
	}
}