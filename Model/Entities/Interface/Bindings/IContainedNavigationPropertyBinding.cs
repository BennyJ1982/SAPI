namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;

	public interface IContainedNavigationPropertyBinding : ISingleNavigationPropertyBinding, IContainedBinding
	{
		IEntity CreateAndSet(IEntity parentEntity, IDictionary<string, IDependency> dependencies);

		void Delete(IEntity parentEntity);
	}
}
