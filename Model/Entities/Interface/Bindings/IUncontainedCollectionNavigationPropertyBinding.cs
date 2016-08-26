namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using Facton.Infrastructure.Entities;

	public interface IUncontainedCollectionNavigationPropertyBinding : ICollectionNavigationPropertyBinding, IUncontainedBinding
	{
		void Add(IEntity parentEntity, IEntity entityToAdd);

		void Remove(IEntity parentEntity, IEntity entityToRemove);
	}
}
