namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	/// <summary>
	/// A data type that can serialize/deserialize values without knowledge of the depending property's actual DomainType
	/// </summary>
	public interface IValueBasedDataType : IDataType
	{
		/// <summary>
		/// Serializes a domain value into its corresponding EDM value.
		/// </summary>
		object Serialize(object domainValue);

		/// <summary>
		/// Deserializes an EDM value into its corresponding domain value.
		/// </summary>
		object Deserialize(object value);
	}
}
