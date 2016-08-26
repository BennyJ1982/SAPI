namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using Facton.Infrastructure.Metadata;

	/// <summary>
	/// A data type which requires a reference to the depending property's actual DomainType when serializing/deserializing a value
	/// </summary>
	public interface IDomainTypeDependingDataType : IDataType
	{
		/// <summary>
		/// Serializes a domain value into its corresponding EDM value.
		/// </summary>
		object Serialize(object domainValue, IDomainType domainType);

		/// <summary>
		/// Deserializes a EDM value into its corresponding domain value.
		/// </summary>
		object Deserialize(object value, IDomainType domainType);
	}
}
