namespace ServiceApi.Server.WebApi.ODataObjects
{
	using System.Web.OData;
	using Microsoft.OData.Edm;
	using ServiceApi.Model.Core.Serialization;

	/// <summary>
	/// Factory for creating odata dto's which, internally, uses web api's edm objects and respective wrappers.
	/// </summary>
	public class WebApiODataObjectFactory : IODataObjectFactory
	{
		public IODataEntityObject CreateODataEntityObject(IEdmEntityType edmType)
		{
			return new ServiceApiEdmEntityObject(edmType);
		}

		public IODataEntityObject CreateODataEntityObject(IEdmEntityTypeReference edmType)
		{
			return new ServiceApiEdmEntityObject(edmType);
		}

		public IODataEntityObject CreateODataEntityObject(IEdmEntityType edmType, bool isNullable)
		{
			return new ServiceApiEdmEntityObject(edmType, isNullable);
		}

		public IODataComplexObject CreateODataComplexObject(IEdmComplexTypeReference edmType)
		{
			return new EdmComplexObject(edmType).ToODataComplexObject();
		}

		public IODataComplexObject CreateODataComplexObject(IEdmComplexType edmType, bool isNullable)
		{
			return new EdmComplexObject(edmType, isNullable).ToODataComplexObject();
		}

		public IODataEntityObjectCollection CreateODataEntityObjectCollection(IEdmCollectionTypeReference edmType)
		{
			return new EdmEntityObjectCollection(edmType).ToODataEntityObjectCollection();
		}

		public IODataComplexObjectCollection CreateODataComplexObjectCollection(IEdmCollectionTypeReference edmType)
		{
			return new EdmComplexObjectCollection(edmType).ToODataComplexObjectCollection();
		}
	}
}
