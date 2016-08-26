namespace ServiceApi.Server.WebApi.ODataObjects.EdmWrappers
{
	using System;
	using System.Web.OData;
	using Microsoft.OData.Edm;
	using ServiceApi.Model.Core.Serialization;

	public abstract class EdmWrapperBase<TEdm> : IODataObject, IEdmObject, IEquatable<EdmWrapperBase<TEdm>>
		where TEdm : IEdmObject
	{
		protected EdmWrapperBase(TEdm edmObject)
		{
			this.UnderlyingEdmObject = edmObject;
		}

		protected internal TEdm UnderlyingEdmObject { get; }

		public IEdmTypeReference GetEdmType()
		{
			return this.UnderlyingEdmObject.GetEdmType();
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as EdmWrapperBase<TEdm>);
		}

		public bool Equals(EdmWrapperBase<TEdm> other)
		{
			return other != null && this.UnderlyingEdmObject.Equals(other.UnderlyingEdmObject);
		}

		public override int GetHashCode()
		{
			return this.UnderlyingEdmObject.GetHashCode();
		}
	}
}
