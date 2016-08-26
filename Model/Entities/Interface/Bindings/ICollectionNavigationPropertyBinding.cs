namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;

	public interface ICollectionNavigationPropertyBinding : INavigationPropertyBinding
	{
		IEnumerable<IEntity> GetAll(IEntity parentEntity);

		bool TryGetByKeys(IEntity parentEntity, IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity);
	}
}
