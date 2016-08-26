namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;

	public interface IEntitySetBinding : ITopLevelElementBinding
	{
		IEnumerable<IEntity> GetAll();

		bool TryGetByKeys(IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity);

		IEntity CreateAndAdd(IDictionary<string, IDependency> dependencies);

		void Delete(IEntity parentEntity, IEntity entityToDelete);
	}
}
