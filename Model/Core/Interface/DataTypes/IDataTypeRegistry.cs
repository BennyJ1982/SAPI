namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using System.Collections.Generic;

	public interface IDataTypeRegistry : IDataTypeLookup
	{
		void Register(IEnumerable<object> keys, IDataType dataType);

		void Register(object key,IDataType dataType);
	}
}
