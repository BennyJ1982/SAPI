namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.Infrastructure.Metadata;

	public interface IModelItemNamingService
	{
		string GetSafeEdmPropertyName(IProperty property);
	}
}