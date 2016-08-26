namespace ServiceApi.Server.WebApi.Deserialization
{
	using System.Web.OData;
	using System.Web.OData.Formatter.Deserialization;
	using Microsoft.OData.Edm;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Server.WebApi.ODataObjects;

	/// <summary>
	/// An odata entity deserializer which, whenever an EdmEntityObject is created, uses our <see cref="ServiceApiEdmEntityObject"/> 
	/// in order to be able to implement <see cref="IODataEntityObject"/>.
	/// </summary>
	public class ServiceApiEntityDeserializer : ODataEntityDeserializer
	{
		public ServiceApiEntityDeserializer(ODataDeserializerProvider deserializerProvider)
			: base(deserializerProvider)
		{
		}

		public override object CreateEntityResource(IEdmEntityTypeReference entityType, ODataDeserializerContext readContext)
		{
			var resource = base.CreateEntityResource(entityType, readContext);

			var edmEntityObject = resource as EdmEntityObject;
			if (edmEntityObject != null)
			{
				// we force web api to use our own EdmEntityObject which implements IODataEntityObject
				resource = new ServiceApiEdmEntityObject(entityType);
			}

			return resource;
		}
	}
}