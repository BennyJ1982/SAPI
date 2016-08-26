namespace Facton.ServiceApi.Domain.Model.Entities.Serialization
{
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;

	public class ODataEntityDtoBuilderFactory : IODataEntityDtoBuilderFactory
	{
		private readonly IODataObjectFactory oDataObjectFactory;

		public ODataEntityDtoBuilderFactory(IODataObjectFactory oDataObjectFactory)
		{
			this.oDataObjectFactory = oDataObjectFactory;
		}

		public IODataEntityDtoBuilder Create(IEntityType entityType)
		{
			return new ODataEntityDtoBuilder(this.oDataObjectFactory, this, entityType);
		}
	}
}