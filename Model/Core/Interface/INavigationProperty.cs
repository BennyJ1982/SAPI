namespace Facton.ServiceApi.Domain.Model.Core
{
	using Microsoft.OData.Edm;

	public interface INavigationProperty : INavigatable
	{
		string Name { get; }

		IEdmNavigationProperty ResultingEdmType { get; }

		IEntityType TargetType { get; }
	}
}