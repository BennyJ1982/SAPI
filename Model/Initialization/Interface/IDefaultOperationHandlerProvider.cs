namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;

	public interface IDefaultOperationHandlerProvider
	{
		IOperationHandler ProvideDefaultGetHandler();

		IOperationHandler ProvideDefaultPatchHandler();

		IOperationHandler ProvideDefaultPostHandler();
	}
}
