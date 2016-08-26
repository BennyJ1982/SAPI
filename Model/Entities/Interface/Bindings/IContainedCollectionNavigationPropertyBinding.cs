namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;

	public interface IContainedCollectionNavigationPropertyBinding : ICollectionNavigationPropertyBinding, IContainedBinding
	{
		IEntity CreateAndAdd(IEntity parentEntity, IDictionary<string, IDependency> dependencies);

		void Delete(IEntity parentEntity, IEntity entityToDelete);
	}
}
