namespace ServiceApi.Server.WebApi.ODataObjects.EdmWrappers
{
	using System.Web.OData;
	using ServiceApi.Model.Core.Serialization;

	/// <summary>
	/// Wraps an EdmComplexObject so it can be used as IODataComplexObject.
	/// </summary>
	internal class EdmComplexObjectWrapper : EdmWrapperBase<EdmComplexObject>, IODataComplexObject, IEdmComplexObject
	{
		public EdmComplexObjectWrapper(EdmComplexObject edmComplexObject) : base(edmComplexObject)
		{
		}

		public bool TryGetPropertyValue(string propertyName, out object value)
		{
			return this.UnderlyingEdmObject.TryGetPropertyValue(propertyName, out value);
		}

		public bool TrySetPropertyValue(string propertyName, object value)
		{
			return this.UnderlyingEdmObject.TrySetPropertyValue(propertyName, value);
		}

		public bool TryGetPropertyValue<T>(string propertyName, out T value)
		{
			return this.UnderlyingEdmObject.TryGetPropertyValue<T>(propertyName, out value);
		}

		public bool TrySetPropertyValue<T>(string propertyName, T value)
		{
			return this.UnderlyingEdmObject.TrySetPropertyValue<T>(propertyName, value);
		}
	}
}
