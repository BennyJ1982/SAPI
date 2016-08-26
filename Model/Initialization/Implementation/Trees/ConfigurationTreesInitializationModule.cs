// <copyright file="ConfigurationTreesInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Module that provides the initializer for configuration trees.
	/// </summary>
	public class ConfigurationTreesInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var propertyService = typeRegistry.GetObject<IPropertyService>();
			var signatureSetInitializer = typeRegistry.GetObject<ISignatureEntitySetInitializer>();
			var configurationRegistry = typeRegistry.GetObject<IModelConfigurationRegistry>();
			var entityTypeInitializerRegistry = typeRegistry.GetObject<IEntityTypeInitializerRegistry>();

			var dataTreeSignatureTypeInitializer = new TreeSignatureTypeInitializer(
				signatureSetInitializer,
				propertyService,
				"Tree",
				"DomainSolution");

			entityTypeInitializerRegistry.RegisterEntityTypeInitializer(dataTreeSignatureTypeInitializer);
			configurationRegistry.RegisterSignatureTypeConfiguration(new SignatureTypeConfiguration("Tree", new[] { "treeNode", "rootNode" }));
		}
	}
}