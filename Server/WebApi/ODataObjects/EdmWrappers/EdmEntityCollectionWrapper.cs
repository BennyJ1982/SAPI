namespace ServiceApi.Server.WebApi.ODataObjects.EdmWrappers
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.OData;
	using Facton.Infrastructure.Core;
	using ServiceApi.Model.Core.Serialization;

	internal class EdmEntityCollectionWrapper : EdmWrapperBase<EdmEntityObjectCollection>, IODataEntityObjectCollection
	{
		public EdmEntityCollectionWrapper(EdmEntityObjectCollection edmObject)
			: base(edmObject)
		{
		}

		public IEnumerator<IODataEntityObject> GetEnumerator()
		{
			return this.UnderlyingEdmObject.Cast<ServiceApiEdmEntityObject>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(IODataEntityObject item)
		{
			this.UnderlyingEdmObject.Add(item.As<ServiceApiEdmEntityObject>());
		}

		public void Clear()
		{
			this.UnderlyingEdmObject.Clear();
		}

		public bool Contains(IODataEntityObject item)
		{
			return this.UnderlyingEdmObject.Contains(item.As<ServiceApiEdmEntityObject>());
		}

		public void CopyTo(IODataEntityObject[] array, int arrayIndex)
		{
			this.UnderlyingEdmObject.Cast<IODataEntityObject>().ToArray().CopyTo(array, arrayIndex);
		}

		public bool Remove(IODataEntityObject item)
		{
			return this.UnderlyingEdmObject.Remove(item.As<ServiceApiEdmEntityObject>());
		}

		public int Count => this.UnderlyingEdmObject.Count;

		public bool IsReadOnly => false;
	}
}
