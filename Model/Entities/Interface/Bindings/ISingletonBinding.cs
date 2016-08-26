namespace Facton.ServiceApi.Domain.Model.Entities.Bindings
{
	using Facton.Infrastructure.Entities;

	public interface ISingletonBinding : ITopLevelElementBinding
	{
		bool TryGet(out IEntity entity);
	}
}
