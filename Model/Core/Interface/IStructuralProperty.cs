namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	public interface IStructuralProperty
	{
		string EdmPropertyName { get; }

		bool CanBeNull { get; }

		IDataType DataType { get; }

		/// <summary>
		/// Serializes the specified value into its odata value using the underlying data type.
		/// </summary>
		object Serialize(object value);

		/// <summary>
		/// Deserializes the specified odata value using the underlying data type.
		/// </summary>
		object Deserialize(object odataValue);
	}
}
