namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using Microsoft.OData.Edm.Library;

	public interface IComplexDataType : IDataType
	{
		void AddToModel(EdmModel model);
	}
}
