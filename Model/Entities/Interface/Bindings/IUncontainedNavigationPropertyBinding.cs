namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using Facton.Infrastructure.Entities;

	public interface IUncontainedNavigationPropertyBinding : ISingleNavigationPropertyBinding, IUncontainedBinding
	{
		void Set(IEntity parentEntity, IEntity entityToSet);

		void Clear(IEntity parentEntity);
	}
}
