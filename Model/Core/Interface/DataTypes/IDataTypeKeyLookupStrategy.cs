namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using System;

	/// <summary>
	/// Describes the strategy used to lookup a key in the data type registry
	/// </summary>
	public interface IDataTypeKeyLookupStrategy
	{
		IDataType GetByKey(Func<object, IDataType> baseLookup, object key);
	}
}
