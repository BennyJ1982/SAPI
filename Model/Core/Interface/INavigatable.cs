namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;

	using Microsoft.OData.Edm;

	public interface INavigatable
	{
		Operation SupportedOperations { get; }

		IEntityType EntityType { get; }

		IEdmEntityType EdmEntityType { get; }
	}
}
