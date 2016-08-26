namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;

	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Edm;

	/// <summary>
	/// Facade providing access to various model-specific objects
	/// </summary>
	public interface IModelContext
	{
		IEdmModel EdmModel { get; }

		bool TryGetEntityType(string name, out IEntityType entityType);

		bool TryGetEntitySet(string name, out IEntitySet entitySet);

		bool TryGetHandler(IOperationContext operationContext, out IOperationHandler relevantHandler);

		// TODO: remove
		INavigatable GetNavigationTarget(ODataPath path);
	}
}
