namespace Facton.ServiceApi.Domain.Model.Entities.Serialization
{
	using Facton.ServiceApi.Domain.Model.Core;

	public interface IODataEntityDtoBuilderFactory 
	{
		IODataEntityDtoBuilder Create(IEntityType entityType);
	}
}