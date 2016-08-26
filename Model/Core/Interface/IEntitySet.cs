namespace Facton.ServiceApi.Domain.Model.Core
{
	using Microsoft.OData.Edm;

	public  interface IEntitySet : INavigatable
	{
		string Name { get; }

		IEdmNavigationSource ResultingEdmType { get; }
	}
}
