namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using Microsoft.OData.Edm;

	public interface IDataType
	{
		IEdmTypeReference GetEdmTypeReference(bool nullable=true);
	}
}
