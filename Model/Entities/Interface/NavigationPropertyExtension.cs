namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	using Microsoft.OData.Edm;

	public static class NavigationPropertyExtension
	{
		public static bool IsContained(this INavigationProperty navigationProperty)
		{
			return navigationProperty.ResultingEdmType.ContainsTarget;
		}

		public static bool IsCollection(this INavigationProperty navigationProperty)
		{
			return navigationProperty.ResultingEdmType.TargetMultiplicity() == EdmMultiplicity.Many;
		}

		public static IEntity CreateEntityInContainedNavigationProperty(
			this INavigationProperty navigationProperty,
			IBindableModelContext context,
			IEntity parentEntity,
			IDictionary<string, IDependency> dependencies)
		{
			if (!navigationProperty.IsContained())
			{
				throw new ArgumentException("Navigation property must be contained.", nameof(navigationProperty));
			}

			if (navigationProperty.IsCollection())
			{
				IContainedCollectionNavigationPropertyBinding binding;
				if (!context.TryGetBinding(navigationProperty, out binding))
				{
					throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
				}

				return binding.CreateAndAdd(parentEntity, dependencies);
			}
			else
			{
				IContainedNavigationPropertyBinding binding;
				if (!context.TryGetBinding(navigationProperty, out binding))
				{
					throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
				}

				return binding.CreateAndSet(parentEntity, dependencies);
			}
		}

		public static bool TryGetNavigationPropertyTarget(
			this INavigationProperty navigationProperty,
			IModelContext context,
			IEntitySet navigationRoot,
			out IEntitySet navigationTarget)
		{
			if (navigationProperty.IsContained())
			{
				throw new ArgumentException("Only uncontained navigation properties can have a navigation target.");
			}

			var edmNavigationTarget = navigationRoot.ResultingEdmType.FindNavigationTarget(navigationProperty.ResultingEdmType);
			if (edmNavigationTarget != null)
			{
				if (context.TryGetEntitySet(edmNavigationTarget.Name, out navigationTarget))
				{
					return true;
				}
			}

			navigationTarget = null;
			return false;
		}
	}
}
