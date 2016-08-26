namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using System.Collections.Generic;

	public interface IDataTypeLookup
	{
		IEnumerable<IDataType> GetAll();

		IDataType GetByKey(object key);
	}
}
