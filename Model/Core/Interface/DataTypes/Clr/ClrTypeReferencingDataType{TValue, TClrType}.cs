namespace Facton.ServiceApi.Domain.Model.Core.DataTypes.Clr
{
	using System;

	using Microsoft.OData.Edm;

	/// <summary>
	/// Simple data type that redirects all operations to a clr data type while applying additional transformations.
	/// </summary>
	public class ClrTypeReferencingDataType<TValue, TClrType> : IValueBasedDataType
	{
		private readonly IClrDataType<TClrType> redirectionTargetDataType;

		private readonly Func<TValue, TClrType> clrValueReader;

		private readonly Func<TClrType, TValue> clrValueWriter;

		public ClrTypeReferencingDataType(Func<TValue, TClrType> clrValueReader, Func<TClrType, TValue> clrValueWriter)
		{
			this.redirectionTargetDataType = ClrDataTypes.GetByClrType<TClrType>();
			this.clrValueReader = clrValueReader;
			this.clrValueWriter = clrValueWriter;
		}

		public object Serialize(object domainValue)
		{
			var clrValue = this.clrValueReader((TValue)domainValue);
			return this.redirectionTargetDataType.Serialize(clrValue);
		}

		public object Deserialize(object edmValue)
		{
			var clrValue = (TClrType)this.redirectionTargetDataType.Deserialize(edmValue);
			return this.clrValueWriter(clrValue);
		}

		public IEdmTypeReference GetEdmTypeReference(bool nullable = true)
		{
			return this.redirectionTargetDataType.GetEdmTypeReference(nullable);
		}
	}
}