namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;

	public interface INavigationPropertyBinder
	{
		void SetUncontainedNavigationProperty(
			IBindableModelContext context,
			IEntity targetEntity,
			INavigationProperty navigationProperty,
			IEnumerable<IEntity> entities);

		void RemoveFromUncontainedNavigationProperty(
			IBindableModelContext context,
			IEntity targetEntity,
			INavigationProperty navigationProperty,
			IEnumerable<IEntity> entitiesToRemove);
	}
}