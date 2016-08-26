namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using Facton.Infrastructure.Entities;

	public interface ISingleNavigationPropertyBinding : INavigationPropertyBinding
	{
		bool TryGet(IEntity parentEntity, out IEntity entity);
	}
}
