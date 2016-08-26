namespace ServiceApi.Server.WebApi.ODataObjects
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.OData;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Server.WebApi.ODataObjects.EdmWrappers;

	internal static class Extensions
	{
		public static IEdmEntityObject ToEdmEntityObject(this IODataEntityObject oDataEntityObject)
		{
			return oDataEntityObject.As<ServiceApiEdmEntityObject>(); 
		}

		public static IODataEntityObjectCollection ToODataEntityObjectCollection(this EdmEntityObjectCollection edmEntityObjectCollection)
		{
			return new EdmEntityCollectionWrapper(edmEntityObjectCollection);
		}

		public static IODataComplexObject ToODataComplexObject(this EdmComplexObject edmComplexObject)
		{
			return new EdmComplexObjectWrapper(edmComplexObject);
		}

		public static IODataComplexObjectCollection ToODataComplexObjectCollection(this EdmComplexObjectCollection edmComplexObjectCollection)
		{
			return new EdmComplexObjectCollectionWrapper(edmComplexObjectCollection);
		}

		public static IEdmComplexObject ToEdmComplexObject(this IODataComplexObject oDataComplexObject)
		{
			return oDataComplexObject.As<EdmComplexObjectWrapper>(); // we know the object must be of our wrapper type, implementing IEdmComplexObject
		}

		public static EdmEntityObjectCollection ToCollection(this IEnumerable<IODataEntityObject> oDataEntities, IEdmEntityType entityType)
		{
			var typeReference = new EdmEntityTypeReference(entityType, false);
			var collectionType = new EdmCollectionType(typeReference);

			return oDataEntities.ToCollection(collectionType);
		}

		public static EdmEntityObjectCollection ToCollection(this IEnumerable<IODataEntityObject> oDataEntities, IEdmCollectionType collectionType)
		{
			var collection = new EdmEntityObjectCollection(new EdmCollectionTypeReference(collectionType));
			collection.AddRange(oDataEntities.Cast<ServiceApiEdmEntityObject>().ToArray());

			return collection;
		}
	}
}
