namespace ServiceApi.Server.WebApi.ODataObjects.EdmWrappers
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.OData;
	using ServiceApi.Model.Core.Serialization;

	internal class EdmComplexObjectCollectionWrapper : EdmWrapperBase<EdmComplexObjectCollection>, IODataComplexObjectCollection
	{
		public EdmComplexObjectCollectionWrapper(EdmComplexObjectCollection edmObject)
			: base(edmObject)
		{
		}

		public IEnumerator<IODataComplexObject> GetEnumerator()
		{
			return this.UnderlyingEdmObject.Cast<EdmComplexObject>().Select(complexObject => complexObject.ToODataComplexObject()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(IODataComplexObject item)
		{
			this.UnderlyingEdmObject.Add(item.ToEdmComplexObject());
		}

		public void Clear()
		{
			this.UnderlyingEdmObject.Clear();
		}

		public bool Contains(IODataComplexObject item)
		{
			return this.UnderlyingEdmObject.Contains(item.ToEdmComplexObject());
		}

		public void CopyTo(IODataComplexObject[] array, int arrayIndex)
		{
			var wrappedArray = array.Select(complexObject => complexObject.ToEdmComplexObject()).ToArray();
			this.UnderlyingEdmObject.CopyTo(wrappedArray, arrayIndex);
		}

		public bool Remove(IODataComplexObject item)
		{
			return this.UnderlyingEdmObject.Remove(item.ToEdmComplexObject());
		}

		public int Count => this.UnderlyingEdmObject.Count;

		public bool IsReadOnly => false;
	}
}
