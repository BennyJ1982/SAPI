// <copyright file="DependencyResolver.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	public class DependencyResolver : IDependencyResolver
	{
		private readonly IUncontainedNavigationPropertyParser navigationPropertyParser;

		public DependencyResolver(IUncontainedNavigationPropertyParser navigationPropertyParser)
		{
			this.navigationPropertyParser = navigationPropertyParser;
		}

		public IDictionary<string, IDependency> ResolveDependencies(
			IBindableModelContext context,
			INavigatable navigationTarget,
			IEntitySet navigationRoot,
			ODataEntityDto oDataEntity)
		{
			//var changedPropertyNames = oDataEntity.GetChangedPropertyNames();
			var changedPropertyNames = oDataEntity.Entry.Properties.Select(p => p.Name).Concat(oDataEntity.NavigationProperties.Select(np => np.Name)).ToArray(); // TODO
			var dependencies = new Dictionary<string, IDependency>();
			foreach (var declaration in context.GetDependencies(navigationTarget))
			{
				IDependency dependency;

				var property = declaration.DependableElement as IStructuralProperty;
				if (property != null && changedPropertyNames.Contains(property.EdmPropertyName)
					&& TryGetPropertyDependency(property, oDataEntity, out dependency))
				{
					dependencies[property.EdmPropertyName] = dependency;
					continue;
				}

				var navigationProperty = declaration.DependableElement as INavigationProperty;
				if (navigationProperty != null && changedPropertyNames.Contains(navigationProperty.Name)
					&& this.TryGetUncontainedNavigationPropertyDependency(context, navigationProperty, navigationRoot, oDataEntity, out dependency))
				{
					dependencies[navigationProperty.Name] = dependency;
					continue;
				}

				if (!declaration.IsOptional)
				{
					throw new NotSupportedException("The dependency is unknown and could not be resolved.");
				}
			}

			return dependencies;
		}

		private static bool TryGetPropertyDependency(IStructuralProperty property, ODataEntityDto oDataEntity, out IDependency dependency)
		{
			object propertyValue;
			if (oDataEntity.TryGetPropertyValue(property, out propertyValue))
			{
				dependency = new PropertyDependency(property, propertyValue);
				return true;
			}

			dependency = null;
			return false;
		}

		private bool TryGetUncontainedNavigationPropertyDependency(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			ODataEntityDto oDataEntity,
			out IDependency dependency)
		{
			if (navigationProperty.IsContained())
			{
				throw new InvalidOperationException("A contained navigation property cannot be included as a dependency.");
			}

			IEnumerable<IEntity> entities;
			if (this.navigationPropertyParser.TryGetLinkedOrInlineEntities(context, oDataEntity, navigationProperty, navigationRoot, out entities))
			{
				dependency = new NavigationPropertyDependency(navigationProperty, entities);
				return true;
			}

			dependency = null;
			return false;
		}
	}
}