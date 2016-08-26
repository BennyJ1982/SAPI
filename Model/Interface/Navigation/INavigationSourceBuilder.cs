namespace Facton.Spikes.ServiceApi.ODataMapping.Model.Navigation
{
	using Facton.Spikes.ServiceApi.ODataMapping.Model.Operations.Handlers;

	public interface INavigationSourceBuilder
	{
		void RegisterOperationHandler(IOperationHandler handler);

		void RegisterDataSourceProvider(IDataSourceProvider provider);
	}
}
