// <copyright file="GlobalInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.ValueRules
{
	using Facton.Domain.Dimensions.Metadata.ValueRules;
	using Facton.Domain.Dimensions.ValueRules;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;

	/// <summary>
	/// Module that provides the initializer for value rules.
	/// </summary>
	public class ValueRulesInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var valueRuleMetadataService = typeRegistry.GetObject<IValueRuleMetadataService>();
			var valueRuleService = typeRegistry.GetObject<IValueRuleService>();
			var propertyService = typeRegistry.GetObject<IPropertyService>();

			var coreTypeInitializerRegistry = typeRegistry.GetObject<IEntityTypeInitializerRegistry>();

			var valueRuleContainerTypeInitializer = new ValueRuleContainerTypeInitializer(
				propertyService,
				valueRuleService,
				valueRuleMetadataService);

			coreTypeInitializerRegistry.RegisterEntityTypeInitializer(valueRuleContainerTypeInitializer);
		}
	}
}