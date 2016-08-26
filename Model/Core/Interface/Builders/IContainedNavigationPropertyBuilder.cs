namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;

	using Microsoft.OData.Edm;

	public interface IContainedNavigationPropertyBuilder : INavigationPropertyBuilder
	{
		IContainedNavigationPropertyBuilder WithSupportedOperations(Operation operation);

		IContainedNavigationPropertyBuilder WithMultiplicity(EdmMultiplicity sourceMultiplicity, EdmMultiplicity targetMultiplicity);
	}
}
