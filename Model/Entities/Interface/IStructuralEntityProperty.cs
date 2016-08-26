namespace Facton.ServiceApi.Domain.Model.Entities
{
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;

	public interface IStructuralEntityProperty : IStructuralProperty
	{
		bool TryGetValueFromEntity(IEntity entity, out object value );

		bool TrySetValueOnEntity(IEntity entity, object value);

		bool IsReadOnly(IEntity targetEntity);
	}
}
