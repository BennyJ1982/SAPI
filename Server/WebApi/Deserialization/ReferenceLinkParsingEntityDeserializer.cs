namespace ServiceApi.Server.WebApi.Deserialization
{
	using System;
	using System.Linq;
	using System.Web.OData;
	using System.Web.OData.Formatter.Deserialization;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Edm;
	using ServiceApi.Server.WebApi.ODataObjects;

	/// <summary>
	/// Entity deserializer which supports parsing entity reference links
	/// </summary>
	public class ReferenceLinkParsingEntityDeserializer : ServiceApiEntityDeserializer
	{
		public ReferenceLinkParsingEntityDeserializer(ODataDeserializerProvider deserializerProvider)
			: base(deserializerProvider)
		{
		}

		public override void ApplyNavigationProperty(
			object entityResource,
			ODataNavigationLinkWithItems navigationLinkWrapper,
			IEdmEntityTypeReference entityType,
			ODataDeserializerContext readContext)
		{
			base.ApplyNavigationProperty(entityResource, navigationLinkWrapper, entityType, readContext);

			var referenceCollection = GetEntityReferences(navigationLinkWrapper, readContext);
			if (referenceCollection.Any())
			{
				if (!entityResource.As<EdmStructuredObject>().TrySetPropertyValue(navigationLinkWrapper.NavigationLink.Name, referenceCollection))
				{
					throw new InvalidOperationException("Unable to set entity reference links as navigation property value.");
				}
			}
		}

		private static EntityReferenceCollection GetEntityReferences(
			ODataNavigationLinkWithItems navigationLinkWrapper,
			ODataDeserializerContext readContext)
		{
			var referenceCollection = new EntityReferenceCollection();
			foreach (var childItem in navigationLinkWrapper.NestedItems.OfType<ODataEntityReferenceLinkBase>())
			{
				var uriParser = readContext.Request.GetODataUriParser(childItem.EntityReferenceLink.Url);
				referenceCollection.Add(uriParser.ParsePath());
			}
			return referenceCollection;
		}
	}
}