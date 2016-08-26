namespace Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers
{
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	public class DefaultOperationHandlerProvider : IDefaultOperationHandlerProvider
	{
		private readonly IOperationHandler getOperationHandler;

		private readonly IOperationHandler patchOperationHandler;

		private readonly IOperationHandler postOperationHandler;

		public DefaultOperationHandlerProvider(
			IEntityReader entityReader,
			IEntityCreator entityCreator,
			IEntityUpdater entityUpdater,
			IODataEntityDtoBuilderFactory dtoBuilderFactory)
		{
			this.getOperationHandler = new GetEntityOperationHandler(entityReader, dtoBuilderFactory);
			this.patchOperationHandler = new PatchEntityOperationHandler(entityReader, entityUpdater, dtoBuilderFactory);
			this.postOperationHandler = new PostEntityOperationHandler(entityReader, entityCreator, dtoBuilderFactory);
		}

		public IOperationHandler ProvideDefaultGetHandler()
		{
			return this.getOperationHandler;
		}

		public IOperationHandler ProvideDefaultPatchHandler()
		{
			return this.patchOperationHandler;
		}

		public IOperationHandler ProvideDefaultPostHandler()
		{
			return this.postOperationHandler;
		}
	}
}
