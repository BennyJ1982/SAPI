namespace ServiceApi.Server.WebApi.ODataObjects
{
	using System.Web.OData;
	using ServiceApi.Server.WebApi.ODataObjects.EdmWrappers;

	internal static class PropertyValueWrappingHelper
	{
		// TODO: In a real implementation this should be replaced with some registry allowing for 
		// registering all edm types that need to be wrapped

		public static bool TryWrapEdmObject(object edmObject, out object dto)
		{
			var edmEntityCollection = edmObject as EdmEntityObjectCollection;
			if (edmEntityCollection != null)
			{
				dto = new EdmEntityCollectionWrapper(edmEntityCollection);
				return true;
			}

			var edmComplexObject = edmObject as EdmComplexObject;
			if (edmComplexObject != null)
			{
				dto = new EdmComplexObjectWrapper(edmComplexObject);
				return true;
			}

			var edmComplexObjectCollection = edmObject as EdmComplexObjectCollection;
			if (edmComplexObjectCollection != null)
			{
				dto = new EdmComplexObjectCollectionWrapper(edmComplexObjectCollection);
				return true;
			}

			dto = null;
			return false;
		}

		public static bool TryUnwrapEdmObject(object dto, out object edmObject)
		{
			var entityCollectionWrapper = dto as EdmEntityCollectionWrapper;
			if (entityCollectionWrapper != null)
			{
				edmObject = entityCollectionWrapper.UnderlyingEdmObject;
				return true;
			}

			var complexWrapper = dto as EdmComplexObjectWrapper;
			if (complexWrapper != null)
			{
				edmObject = complexWrapper.UnderlyingEdmObject;
				return true;
			}

			var complexCollectionWrapper = dto as EdmComplexObjectCollectionWrapper;
			if (complexCollectionWrapper != null)
			{
				edmObject = complexCollectionWrapper.UnderlyingEdmObject;
				return true;
			}

			edmObject = null;
			return false;
		}
	}
}
