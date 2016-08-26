namespace Facton.ServiceApi.Domain.Model.Initialization.DataTypes
{
	using System;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;

	/// <summary>
	/// Data type which can handle FACTON domain types whose values can be serialized/deserialized from strings
	/// </summary>
	public class SerializableDomainTypeDataType : IDomainTypeDependingDataType
	{
		public IEdmTypeReference GetEdmTypeReference(bool nullable = true)
		{
			return EdmCoreModel.Instance.GetPrimitive(EdmPrimitiveTypeKind.String, nullable);
		}

		public object Serialize(object domainValue, IDomainType domainType)
		{
			int attributes;
			return domainType.SerializeValue(domainValue, out attributes);
		}

		public object Deserialize(object value, IDomainType domainType)
		{
			object deserializedValue;
			if (domainType.TryDeserializeValue(Convert.ToString(value), 0, out deserializedValue))
			{
				return deserializedValue;
			}

			throw new ArgumentException("Cannot deserialize value", nameof(value));
		}
	}
}
