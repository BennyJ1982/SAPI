namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	public class NavigationPropertyBinder : INavigationPropertyBinder
	{
		public void SetUncontainedNavigationProperty(
			IBindableModelContext context,
			IEntity targetEntity,
			INavigationProperty navigationProperty,
			IEnumerable<IEntity> entities)
		{
			ThrowIfContained(navigationProperty);
			if (navigationProperty.IsCollection())
			{
				IUncontainedCollectionNavigationPropertyBinding binding;
				if (context.TryGetBinding(navigationProperty, out binding))
				{
					// TODO: think about having an AddRange method for better performance
					entities.ForEach(entity => binding.Add(targetEntity, entity));
					return;
				}
			}
			else
			{
				IUncontainedNavigationPropertyBinding binding;
				if (context.TryGetBinding(navigationProperty, out binding))
				{
					binding.Set(targetEntity, entities.Single());
					return;
				}
			}

			throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
		}

		public void RemoveFromUncontainedNavigationProperty(
			IBindableModelContext context,
			IEntity targetEntity,
			INavigationProperty navigationProperty,
			IEnumerable<IEntity> entitiesToRemove)
		{
			ThrowIfContained(navigationProperty);
			if (navigationProperty.IsCollection())
			{
				IUncontainedCollectionNavigationPropertyBinding binding;
				if (context.TryGetBinding(navigationProperty, out binding))
				{
					entitiesToRemove.ForEach(entityToRemove => binding.Remove(targetEntity, entityToRemove));
					return;
				}
			}
			else
			{
				IUncontainedNavigationPropertyBinding binding;
				if (context.TryGetBinding(navigationProperty, out binding))
				{
					binding.Clear(targetEntity);
					return;
				}
			}

			throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
		}

		private static void ThrowIfContained(INavigationProperty navigationProperty)
		{
			if (navigationProperty.IsContained())
			{
				throw new ArgumentException("Navigation property must be uncontained.", nameof(navigationProperty));
			}
		}
	}
}
