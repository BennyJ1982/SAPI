// <copyright file="EntitySetBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using System;
	using System.Collections.Generic;
	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;

	public class EntitySetBuilder : IEntitySetBuilder
	{
		private readonly IDictionary<IUncontainedNavigationPropertyBuilder, IEntitySetBuilder> uncontainedNavigationPropertyTargets =
			new Dictionary<IUncontainedNavigationPropertyBuilder, IEntitySetBuilder>();

		private readonly EdmModel edmModel;

		private readonly string name;

		private Operation supportedOperations;

		private bool uncontainedNavigationPropertyTargetsAlreadyBuilt;

		public EntitySetBuilder(string name, IEntityTypeBuilder containedEntityTypeBuilder, EdmModel edmModel)
		{
			this.edmModel = edmModel;
			this.name = name;
			this.ContainedEntityType = containedEntityTypeBuilder;
		}

		public bool AlreadyBuilt => this.BuiltEntitySet != null;

		public INavigatable BuiltElement => this.BuiltEntitySet;

		public bool IsSingleton { get; private set; }

		public IEntitySet BuiltEntitySet { get; private set; }

		public IEntityTypeBuilder ContainedEntityType { get; }

		public IEntitySetBuilder AsSingleton()
		{
			this.ThrowIfAlreadyBuilt();
			this.IsSingleton = true;
			return this;
		}

		public IEntitySetBuilder WithSupportedOperations(Operation operation)
		{
			this.ThrowIfAlreadyBuilt();
			this.supportedOperations = operation;
			return this;
		}

		public IEntitySetBuilder WithUncontainedNavigationPropertyTarget(
			IUncontainedNavigationPropertyBuilder navigationProperty,
			IEntitySetBuilder targetBuilder)
		{
			this.ThrowIfAlreadyBuilt();
			// TODO: recursive check necessary
			//if (!this.ContainedEntityType.NavigationPropertyBuilders.Contains(navigationProperty))
			//{
			//	throw new ArgumentException("The specified navigation property is not part of this set's entity type.", nameof(navigationProperty));
			//}

			this.uncontainedNavigationPropertyTargets[navigationProperty] = targetBuilder;
			return this;
		}

		public IEntitySet Build()
		{
			this.ThrowIfAlreadyBuilt();
			if (this.supportedOperations == 0)
			{
				throw new InvalidOperationException("An entity set has to support at least one operation.");
			}


			if (!this.ContainedEntityType.AlreadyBuilt)
			{
				throw new InvalidOperationException("The contained entity type hasn't been built yet.");
			}

			var container = this.edmModel.EntityContainer.As<EdmEntityContainer>();
			var edmEntitySet = this.CreateEdmType(container);
			var entitySet = new EntitySet(edmEntitySet, this.ContainedEntityType.BuiltEntityType, this.supportedOperations);

			return this.BuiltEntitySet = entitySet;
		}

		public void BuildUncontainedNavigationPropertyTargets()
		{
			if (!this.AlreadyBuilt)
			{
				throw new InvalidOperationException("Entity set has to be built before building its uncontained navigation property targets.");
			}

			if (this.uncontainedNavigationPropertyTargetsAlreadyBuilt)
			{
				throw new InvalidOperationException("Uncontained navigation property targets have already been built for this entity set.");
			}

			var navigationSource = this.BuiltEntitySet.ResultingEdmType.As<EdmNavigationSource>();
			foreach (var uncontainedTarget in this.uncontainedNavigationPropertyTargets)
			{
				if (!uncontainedTarget.Value.AlreadyBuilt)
				{
					throw new InvalidOperationException("The uncontained navigation property target entity set hasn't been built yet.");
				}

				navigationSource.AddNavigationTarget(
					uncontainedTarget.Key.BuiltNavigationProperty.ResultingEdmType,
					uncontainedTarget.Value.BuiltEntitySet.ResultingEdmType);
			}

			this.uncontainedNavigationPropertyTargetsAlreadyBuilt = true;
		}


		private IEdmNavigationSource CreateEdmType(EdmEntityContainer container)
		{
			EdmNavigationSource edmNavigationSource;
			if (this.IsSingleton)
			{
				edmNavigationSource = container.AddSingleton(this.name, this.ContainedEntityType.BuiltEntityType.ResultingEdmEntityType);
			}
			else
			{
				edmNavigationSource = container.AddEntitySet(this.name, this.ContainedEntityType.BuiltEntityType.ResultingEdmEntityType);
			}

			return edmNavigationSource;
		}

		private void ThrowIfAlreadyBuilt()
		{
			if (this.AlreadyBuilt)
			{
				throw new InvalidOperationException("This entity set has already been built.");
			}
		}
	}
}