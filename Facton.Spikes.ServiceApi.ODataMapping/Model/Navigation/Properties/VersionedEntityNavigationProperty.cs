namespace Facton.Spikes.ServiceApi.ODataMapping.Model.Navigation.Properties
{
	using Facton.Spikes.ServiceApi.ODataMapping.Model.EntityTypes.Interface;
	using Facton.Spikes.ServiceApi.ODataMapping.Model.Operations;
	using Facton.Spikes.ServiceApi.ODataMapping.Queries.DataProviders;

	internal class VersionedEntityNavigationProperty : NavigationProperty
	{
		private readonly IEntityDataProviderFactory dataProviderFactory;


		public VersionedEntityNavigationProperty(
			IEntityType sourceType,
			IEntityType targetType,
			string sourcePropertyName,
			IEntityDataProviderFactory dataProviderFactory)
			: base(sourceType, targetType, sourcePropertyName)
		{
			this.dataProviderFactory = dataProviderFactory;
		}

		public VersionedEntityNavigationProperty(
			IEntityType sourceType,
			IEntityType targetType,
			string sourcePropertyName,
			string targetPropertyName,
			IEntityDataProviderFactory dataProviderFactory)
			: base(sourceType, targetType, sourcePropertyName, targetPropertyName)
		{
			this.dataProviderFactory = dataProviderFactory;
		}

		public override IEntityDataProvider GetDataProvider(IEntityDataProvider preceedingProvider, IInternalCrudOperationContext context)
		{
			// the actual provider for the entity that we want to query here comes from the path segment preceeding the version
			var entityDataProvider = this.ModelContext.GetDataProviderChain(context.StepBack(2));

			return this.dataProviderFactory.CreateVersionedEntityProvider(preceedingProvider, entityDataProvider);
		}
	}
}
