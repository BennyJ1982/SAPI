namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;

	using Microsoft.OData.Edm;

	public interface IUncontainedNavigationPropertyBuilder : INavigationPropertyBuilder
	{
		IUncontainedNavigationPropertyBuilder WithSupportedOperations(Operation operation);

		IUncontainedNavigationPropertyBuilder WithMultiplicity(EdmMultiplicity sourceMultiplicity, EdmMultiplicity targetMultiplicity);
	}
}
