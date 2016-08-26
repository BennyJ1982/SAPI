// <copyright file="EntityTypeBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Builders.Navigation;
	using Microsoft.OData.Edm.Library;

	public class EntityTypeBuilder : IEntityTypeBuilder
	{
		private readonly INavigationPropertyBuilderFactory navigationPropertyBuilderFactory;

		private readonly Dictionary<string, IStructuralProperty> properties = new Dictionary<string, IStructuralProperty>();

		private readonly Dictionary<string, IStructuralProperty> keyProperties = new Dictionary<string, IStructuralProperty>();

		private readonly Dictionary<string, INavigationPropertyBuilder> navigationPropertyBuilders =
			new Dictionary<string, INavigationPropertyBuilder>();

		private readonly EdmModel edmModel;

		private readonly string name;

		private bool isOpen;

		private bool isAbstract;

		private bool isBuilding;

		private EntityType builtEntityType;

		public EntityTypeBuilder(INavigationPropertyBuilderFactory navigationPropertyBuilderFactory, string name, EdmModel edmModel)
		{
			this.edmModel = edmModel;
			this.navigationPropertyBuilderFactory = navigationPropertyBuilderFactory;
			this.name = name;
		}

		public bool AlreadyBuilt => this.BuiltEntityType != null;

		public bool NavigationPropertiesAlreadyBuilt { get; private set; }

		public IEntityType BuiltEntityType => this.builtEntityType;

		public IEnumerable<IStructuralProperty> KeyProperties => this.keyProperties.Values;

		public IEnumerable<IStructuralProperty> Properties => this.properties.Values;

		public IEntityTypeBuilder ParentTypeBuilder { get; private set; }

		public IEntityTypeBuilder AsOpenType()
		{
			this.ThrowIfAlreadyBuilt();
			this.isOpen = true;
			return this;
		}

		public IEntityTypeBuilder AsAbstractType()
		{
			this.ThrowIfAlreadyBuilt();
			this.isAbstract = true;
			return this;
		}

		public IEntityTypeBuilder WithParentType(IEntityTypeBuilder parentTypeBuilder)
		{
			this.ThrowIfAlreadyBuilt();
			this.ParentTypeBuilder = parentTypeBuilder;
			return this;
		}

		public IEntityTypeBuilder WithKeyProperty(IStructuralProperty property)
		{
			this.ThrowIfAlreadyBuilt();
			this.ThrowIfDuplicatePropertyName(property.EdmPropertyName);
			this.keyProperties.Add(property.EdmPropertyName, property);
			return this;
		}

		public IEntityTypeBuilder WithStructuralProperty(IStructuralProperty property)
		{
			this.ThrowIfAlreadyBuilt();
			this.ThrowIfDuplicatePropertyName(property.EdmPropertyName);
			this.properties.Add(property.EdmPropertyName, property);
			return this;
		}

		public IEnumerable<INavigationPropertyBuilder> NavigationPropertyBuilders => this.navigationPropertyBuilders.Values;

		public IContainedNavigationPropertyBuilder CreateContainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, string propertyName)
		{
			this.ThrowIfAlreadyBuilt();
			this.ThrowIfDuplicatePropertyName(propertyName);

			var builder = this.navigationPropertyBuilderFactory.CreateContainedNavigationPropertyBuilder(targetTypeBuilder, propertyName);
			this.navigationPropertyBuilders.Add(propertyName, builder);

			return builder;
		}

		public IContainedNavigationPropertyBuilder CreateContainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, IProperty propertyName)
		{
			this.ThrowIfAlreadyBuilt();

			var builder = this.navigationPropertyBuilderFactory.CreateContainedNavigationPropertyBuilder(targetTypeBuilder, propertyName);
			this.ThrowIfDuplicatePropertyName(builder.SourcePropertyName);
			this.navigationPropertyBuilders.Add(builder.SourcePropertyName, builder);

			return builder;
		}

		public IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, string propertyName)
		{
			this.ThrowIfAlreadyBuilt();
			this.ThrowIfDuplicatePropertyName(propertyName);

			var builder = this.navigationPropertyBuilderFactory.CreateUncontainedNavigationPropertyBuilder(targetTypeBuilder, propertyName);
			this.navigationPropertyBuilders.Add(propertyName, builder);

			return builder;
		}

		public IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, IProperty propertyName)
		{
			this.ThrowIfAlreadyBuilt();

			var builder = this.navigationPropertyBuilderFactory.CreateUncontainedNavigationPropertyBuilder(targetTypeBuilder, propertyName);
			this.ThrowIfDuplicatePropertyName(builder.SourcePropertyName);
			this.navigationPropertyBuilders.Add(builder.SourcePropertyName, builder);

			return builder;
		}

		public IEntityType Build()
		{
			this.ThrowIfAlreadyBuilt();
			this.ThrowIfBuilding();

			if (string.IsNullOrEmpty(this.name))
			{
				throw new InvalidOperationException("Name must not be emtpy.");
			}

			if (this.keyProperties.IsEmpty() && this.ParentTypeBuilder == null)
			{
				throw new InvalidOperationException("Entity type must have a key or a parent entity type.");
			}

			if (this.keyProperties.Any() && this.ParentTypeBuilder != null)
			{
				throw new InvalidOperationException("Entity type must not have a key and a parent entity type.");
			}

			this.ThrowIfDuplicatePropertiesInHierarchy();
			this.isBuilding = true;

			try
			{
				var edmEntityType = this.BuildEdmEntityType();
				return this.builtEntityType =
					new EntityType(this.name, edmEntityType, this.GetEffectiveKeyProperties(), this.GetEffectiveProperties());
			}
			finally
			{
				this.isBuilding = false;
			}
		}

		public void BuildNavigationProperties()
		{
			if (!this.AlreadyBuilt)
			{
				throw new InvalidOperationException("Entity type has to be built before building its navigation properties.");
			}

			if (this.NavigationPropertiesAlreadyBuilt)
			{
				throw new InvalidOperationException("Navigation properties have already been built for this entity type.");
			}

			// build navigation properties (which adds them to the already created edm type)
			this.navigationPropertyBuilders.Values.Where(n => !n.AlreadyBuilt)
				.ForEach(n => n.Build(this.builtEntityType.ResultingEdmEntityType.As<EdmEntityType>()));

			// ensure navigation properties of parent type have also been built
			if (this.ParentTypeBuilder != null && !this.ParentTypeBuilder.NavigationPropertiesAlreadyBuilt)
			{
				this.ParentTypeBuilder.BuildNavigationProperties();
			}

			// now add all effective navigation properties to our entity type
			this.builtEntityType.SetNavigationProperties(this.GetEffectiveNavigationProperties().Select(p => p.BuiltNavigationProperty));
			this.NavigationPropertiesAlreadyBuilt = true;
		}

		private EdmEntityType BuildEdmEntityType()
		{
			IEntityType parentType = null;
			if (this.ParentTypeBuilder != null)
			{
				parentType = this.ParentTypeBuilder.AlreadyBuilt ? this.ParentTypeBuilder.BuiltEntityType : this.ParentTypeBuilder.Build();
			}

			var edmEntityType = new EdmEntityType("facton", this.name, parentType?.ResultingEdmEntityType, this.isAbstract, this.isOpen);
			if (this.ParentTypeBuilder == null)
			{
				this.AddKeyPropertiesToEdmType(edmEntityType);
			}

			this.AddPropertiesToEdmType(edmEntityType);
			this.edmModel.AddElement(edmEntityType);

			return edmEntityType;
		}

		private void AddPropertiesToEdmType(EdmStructuredType entityType)
		{
			foreach (var property in this.properties.Values)
			{
				entityType.AddStructuralProperty(property.EdmPropertyName, property.DataType.GetEdmTypeReference(property.CanBeNull));
			}
		}

		private void AddKeyPropertiesToEdmType(EdmEntityType edmEntityType)
		{
			foreach (var keyProperty in this.keyProperties.Values)
			{
				var id = edmEntityType.AddStructuralProperty(
					keyProperty.EdmPropertyName,
					keyProperty.DataType.GetEdmTypeReference(keyProperty.CanBeNull));
				edmEntityType.AddKeys(id);
			}
		}

		private IReadOnlyDictionary<string, IStructuralProperty> GetEffectiveKeyProperties()
		{
			IEntityTypeBuilder entityType = this;
			while (entityType.KeyProperties.IsEmpty())
			{
				entityType = entityType.ParentTypeBuilder;
			}

			return entityType.KeyProperties.ToDictionary(p => p.EdmPropertyName, p => p);
		}

		private IReadOnlyDictionary<string, IStructuralProperty> GetEffectiveProperties()
		{
			var effectiveProperties = new Dictionary<string, IStructuralProperty>();
			IEntityTypeBuilder entityType = this;

			do
			{
				entityType.Properties.ForEach(property => effectiveProperties[property.EdmPropertyName] = property);
				entityType = entityType.ParentTypeBuilder;
			}
			while (entityType != null);

			return effectiveProperties;
		}

		private void ThrowIfAlreadyBuilt()
		{
			if (this.AlreadyBuilt)
			{
				throw new InvalidOperationException("This entity type has already been built.");
			}
		}

		private void ThrowIfBuilding()
		{
			if (this.isBuilding)
			{
				throw new InvalidOperationException("This entity type is already being built.");
			}
		}

		private void ThrowIfDuplicatePropertyName(string sourcePropertyName)
		{
			if (this.navigationPropertyBuilders.ContainsKey(sourcePropertyName))
			{
				throw new ArgumentException("Navigation property with the same name already exists.", nameof(sourcePropertyName));
			}

			if (this.properties.ContainsKey(sourcePropertyName))
			{
				throw new ArgumentException("Property with the same name already exists.", nameof(sourcePropertyName));
			}

			if (this.keyProperties.ContainsKey(sourcePropertyName))
			{
				throw new ArgumentException("Key property with the same name already exists.", nameof(sourcePropertyName));
			}
		}

		private void ThrowIfDuplicatePropertiesInHierarchy()
		{
			var names = new HashSet<string>();
			IEntityTypeBuilder typeBuilder = this;
			while (typeBuilder != null)
			{
				foreach (var keyProperty in typeBuilder.KeyProperties.Select(p => p.EdmPropertyName))
				{
					if (names.Contains(keyProperty))
					{
						throw new InvalidOperationException("Key property " + keyProperty + " is already contained in one of the parent types.");
					}
					names.Add(keyProperty);
				}

				foreach (var property in typeBuilder.Properties.Select(p => p.EdmPropertyName))
				{
					if (names.Contains(property))
					{
						throw new InvalidOperationException("Property " + property + " is already contained in one of the parent types.");
					}
					names.Add(property);
				}

				foreach (var navigationProperty in typeBuilder.NavigationPropertyBuilders.Select(n => n.SourcePropertyName))
				{
					if (names.Contains(navigationProperty))
					{
						throw new InvalidOperationException("Property " + navigationProperty + " is already contained in one of the parent types.");
					}
					names.Add(navigationProperty);
				}

				typeBuilder = typeBuilder.ParentTypeBuilder;
			}
		}
	}
}