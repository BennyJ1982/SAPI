// <copyright file="ModelBuilder.cs" company="Facton GmbH">
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
	using Facton.ServiceApi.Domain.Model.Core.Builders.Navigation;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;

	public class ModelBuilder : IModelBuilder
	{
		private readonly IODataPathService pathTranslator;

		private readonly INavigationPropertyBuilderFactory navigationPropertyBuilderFactory;

		private readonly Dictionary<string, IEntityTypeBuilder> entityTypeBuilders = new Dictionary<string, IEntityTypeBuilder>();

		private readonly Dictionary<string, IEntitySetBuilder> entitySetBuilders = new Dictionary<string, IEntitySetBuilder>();

		private readonly IList<IComplexDataType> complexDataTypes = new List<IComplexDataType>();

		private readonly IList<IOperationHandler> globalOperationHandlers = new List<IOperationHandler>();

		private readonly IList<KeyValuePair<INavigatableElementBuilder, IOperationHandler>> operationHandlers =
			new List<KeyValuePair<INavigatableElementBuilder, IOperationHandler>>();

		private readonly EdmModel edmModelUnderConstruction;

		private bool alreadyBuilt;

		private string nameSpace;

		private string containerName;

		public ModelBuilder(IODataPathService pathTranslator, INavigationPropertyBuilderFactory navigationPropertyBuilderFactory)
		{
			this.pathTranslator = pathTranslator;
			this.navigationPropertyBuilderFactory = navigationPropertyBuilderFactory;
			this.edmModelUnderConstruction = new EdmModel();
		}

		public IEnumerable<IEntityTypeBuilder> EntityTypeBuilders => this.entityTypeBuilders.Values;

		public IEnumerable<IEntitySetBuilder> EntitySetBuilders => this.entitySetBuilders.Values;

		public IModelBuilder WithNamespace(string nameSpace)
		{
			this.nameSpace = nameSpace;
			return this;
		}

		public IModelBuilder WithContainerName(string containerName)
		{
			this.containerName = containerName;
			return this;
		}

		public IModelBuilder WithComplexDataType(IComplexDataType complexDataType)
		{
			this.complexDataTypes.Add(complexDataType);
			return this;
		}

		public IModelBuilder WithOperationHandler(INavigatableElementBuilder navigatableElementBuilder, IOperationHandler operationHandler)
		{
			this.operationHandlers.Add(new KeyValuePair<INavigatableElementBuilder, IOperationHandler>(navigatableElementBuilder, operationHandler));
			return this;
		}

		public IModelBuilder WithOperationHandler(IOperationHandler operationHandler)
		{
			this.globalOperationHandlers.Add(operationHandler);
			return this;
		}

		public IEntityTypeBuilder CreateEntityTypeBuilder(string name)
		{
			if (this.entityTypeBuilders.ContainsKey(name))
			{
				throw new ArgumentException("Entity type builder already exists.", nameof(name));
			}

			return this.entityTypeBuilders[name] = new EntityTypeBuilder(this.navigationPropertyBuilderFactory, name, this.edmModelUnderConstruction);
		}

		public IEntitySetBuilder CreateEntitySetBuilder(string name, IEntityTypeBuilder containedEntityTypeBuilder)
		{
			if (this.entitySetBuilders.ContainsKey(name))
			{
				throw new ArgumentException("Entity set builder already exists.", nameof(name));
			}

			return this.entitySetBuilders[name] = new EntitySetBuilder(name, containedEntityTypeBuilder, this.edmModelUnderConstruction);
		}

		public bool TryGetEntityTypeBuilder(string name, out IEntityTypeBuilder entityTypeBuilder)
		{
			return this.entityTypeBuilders.TryGetValue(name, out entityTypeBuilder);
		}

		public bool TryGetEntitySetBuilder(string name, out IEntitySetBuilder entitySetBuilder)
		{
			return this.entitySetBuilders.TryGetValue(name, out entitySetBuilder);
		}

		public IModelContext Build()
		{
			if (this.alreadyBuilt)
			{
				throw new InvalidOperationException("The model has been already built");
			}

			if (string.IsNullOrEmpty(this.nameSpace))
			{
				throw new InvalidOperationException("Namespace hasn't been set");
			}

			if (string.IsNullOrEmpty(this.containerName))
			{
				throw new InvalidOperationException("Container name hasn't been set");
			}


			var edmModel = this.BuildEdmModel();
			var context = new ModelContext(
				edmModel,
				this.entityTypeBuilders.ToDictionary(e => e.Key, e => e.Value.BuiltEntityType),
				this.entitySetBuilders.ToDictionary(e => e.Key, e => e.Value.BuiltEntitySet),
				this.pathTranslator,
				this.GetOperationHandlerStore());

			this.alreadyBuilt = true;
			return context;
		}

		private IEdmModel BuildEdmModel()
		{
			var container = new EdmEntityContainer(this.nameSpace, this.containerName);
			this.edmModelUnderConstruction.AddElement(container);

			this.BuildComplexDataTypes();
			this.BuildEntityTypes();
			this.BuildNavigationProperties();
			this.BuildEntitySets();
			this.BuildUncontainedNavigationPropertyTargets();

			return this.edmModelUnderConstruction;
		}

		private void BuildComplexDataTypes()
		{
			this.complexDataTypes.ForEach(dataType => dataType.AddToModel(this.edmModelUnderConstruction));
		}

		private void BuildEntityTypes()
		{
			foreach (var entityTypeBuilder in this.entityTypeBuilders.Values)
			{
				if (!entityTypeBuilder.AlreadyBuilt)
				{
					entityTypeBuilder.Build();
				}
			}
		}

		private void BuildNavigationProperties()
		{
			foreach (var entityTypeBuilder in this.entityTypeBuilders.Values)
			{
				if (!entityTypeBuilder.NavigationPropertiesAlreadyBuilt)
				{
					entityTypeBuilder.BuildNavigationProperties();
				}
			}
		}

		private void BuildUncontainedNavigationPropertyTargets()
		{
			foreach (var entitySetBuilder in this.entitySetBuilders.Values)
			{
				entitySetBuilder.BuildUncontainedNavigationPropertyTargets();
			}
		}

		private void BuildEntitySets()
		{
			this.entitySetBuilders.Values.ForEach(entitySet => entitySet.Build());
		}

		private OperationHandlerStore GetOperationHandlerStore()
		{
			var navigationTargetHandlers =
				this.operationHandlers.Select(
					pair => new KeyValuePair<INavigatable, IOperationHandler>(pair.Key.BuiltElement, pair.Value));

			return new OperationHandlerStore(navigationTargetHandlers, this.globalOperationHandlers);
		}
	}
}