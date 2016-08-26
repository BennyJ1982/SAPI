namespace Facton.ServiceApi.Domain.Model.Core.DataTypes.Clr
{
	using System;

	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.EdmToClrConversion;
	using Microsoft.OData.Edm.Library;
	using Microsoft.OData.Edm.Values;

	public class ClrDataType<T> : IClrDataType<T>
	{
		private readonly EdmPrimitiveTypeKind edmPrimitiveType;

		private readonly EdmToClrConverter converter = new EdmToClrConverter();

		public ClrDataType(EdmPrimitiveTypeKind edmPrimitiveType)
		{
			this.edmPrimitiveType = edmPrimitiveType;
		}

		public virtual object Serialize(object domainValue)
		{
			return domainValue; // we rely on odata's built-in clr type mapper to handle this correctly 
		}

		public virtual object Deserialize(object value)
		{
			var edmValue = value as IEdmValue;
			if (edmValue != null)
			{
				return this.converter.AsClrValue<T>(edmValue);
			}

			return (T)Convert.ChangeType(value, typeof(T));
		}

		public IEdmTypeReference GetEdmTypeReference(bool nullable = true)
		{
			return EdmCoreModel.Instance.GetPrimitive(this.edmPrimitiveType, nullable);
		}
	}
}
