namespace ServiceApi.Server.WebApi.Serializers
{
	using System.Web.OData.Formatter.Serialization;
	using Microsoft.OData.Edm;

	public class SerializerProvider : DefaultODataSerializerProvider
	{
		private readonly ODataEntityTypeSerializer entityTypeSerializer;

		public SerializerProvider()
		{
			this.entityTypeSerializer = new NotProvidedFilteringEntityTypeSerializer(this);
		}

		public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
		{
			return edmType.IsEntity() ? this.entityTypeSerializer : base.GetEdmTypeSerializer(edmType);
		}
	}
}