namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using System;

	using Facton.Infrastructure.Metadata;

	public static class DataTypeExtension
	{
		public static object Deserialize(this IDataType dataType, object value, IDomainType domainType)
		{
			var valueBasedDataType = dataType as IValueBasedDataType;
			if (valueBasedDataType != null)
			{
				return valueBasedDataType.Deserialize(value);
			}

			var domainTypeBasedDataType = dataType as IDomainTypeDependingDataType;
			if (domainTypeBasedDataType != null)
			{
				return domainTypeBasedDataType.Deserialize(value, domainType);
			}

			throw new InvalidOperationException("This data type does not provide a know way of deserializing values.");
		}

		public static object Serialize(this IDataType dataType, object domainValue, IDomainType domainType)
		{
			var valueBasedDataType = dataType as IValueBasedDataType;
			if (valueBasedDataType != null)
			{
				return valueBasedDataType.Serialize(domainValue);
			}

			var domainTypeBasedDataType = dataType as IDomainTypeDependingDataType;
			if (domainTypeBasedDataType != null)
			{
				return domainTypeBasedDataType.Serialize(domainValue, domainType);
			}

			throw new InvalidOperationException("This data type does not provide a know way of serializing values.");
		}
	}
}
