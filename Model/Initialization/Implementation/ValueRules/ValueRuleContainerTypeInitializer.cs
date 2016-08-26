// <copyright file="ValueRuleContainerTypeInitializer.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.ValueRules
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Domain.Dimensions.Metadata.ValueRules;
	using Facton.Domain.Dimensions.ValueRules;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;

	using Microsoft.OData.Edm;

	/// <summary>
	/// Initializes the value rule container type and adds it as navigation property to the public entity type.
	/// </summary>
	public class ValueRuleContainerTypeInitializer : ICoreTypeInitializer
	{
		private readonly IPropertyService propertyService;

		private readonly IValueRuleService valueRuleService;

		private readonly IValueRuleMetadataService valueRuleMetadataService;

		public ValueRuleContainerTypeInitializer(
			IPropertyService propertyService,
			IValueRuleService valueRuleService,
			IValueRuleMetadataService valueRuleMetadataService)
		{
			this.propertyService = propertyService;
			this.valueRuleService = valueRuleService;
			this.valueRuleMetadataService = valueRuleMetadataService;
		}

		public IEntityTypeBuilder Initialize(IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType)
		{
			var valueRuleBuilder = modelBuilder.CreateEntityTypeBuilder(FactonModelKeywords.ValueRuleEntityTypeName).WithParentType(publicEntityType);

			var valueRuleContainerBuilder = modelBuilder.CreateEntityTypeBuilder(FactonModelKeywords.ValueRuleContainerTypeName);
			publicEntityType.KeyProperties.ForEach(keyProperty => valueRuleContainerBuilder.WithKeyProperty(keyProperty));

			foreach (var valueProperty in this.GetDimensionDependingProperties())
			{
				var valueRuleProperty =
					valueRuleContainerBuilder.CreateContainedNavigationProperty(valueRuleBuilder, valueProperty)
						.WithMultiplicity(EdmMultiplicity.One, EdmMultiplicity.Many)
						.WithSupportedOperations(Operation.Get | Operation.Post | Operation.Patch);

				var binding = new ConcreteValueRuleContainerNavigationPropertyBinding(
					this.valueRuleService,
					this.valueRuleMetadataService,
					valueProperty);

				modelBuilder
					.WithBinding(valueRuleProperty, binding)
					.WithOptionalDependency(valueRuleProperty, publicEntityType.KeyProperties.First());

			}

			AddContainerNavigationProperty(modelBuilder, publicEntityType, valueRuleContainerBuilder);
			return valueRuleContainerBuilder;
		}

		private static void AddContainerNavigationProperty(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder publicEntityType,
			IEntityTypeBuilder containerEntityType)
		{
			modelBuilder.WithBinding(
				publicEntityType.CreateContainedNavigationProperty(containerEntityType, FactonModelKeywords.ValueRuleContainerNavigationPropertyName)
					.WithMultiplicity(EdmMultiplicity.One, EdmMultiplicity.One)
					.WithSupportedOperations(Operation.Get),
				new VirtualValueRuleContainerNavigationPropertyBinding());
		}

		private IEnumerable<IProperty> GetDimensionDependingProperties()
		{
			return this.propertyService.AllRelevantProperties.Where(this.IsValueProperty);
		}

		private bool IsValueProperty(IProperty p)
		{
			return this.valueRuleMetadataService.AllDimensionalProperties.Contains(p);
		}
	}
}