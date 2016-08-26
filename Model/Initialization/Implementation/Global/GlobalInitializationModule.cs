// <copyright file="GlobalInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Global
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Module that provides the initializer for global entities.
	/// </summary>
	public class GlobalInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var signatureSetInitializer = typeRegistry.GetObject<ISignatureEntitySetInitializer>();
			var configurationRegistry = typeRegistry.GetObject<IModelConfigurationRegistry>();
			var entityTypeInitializerRegistry = typeRegistry.GetObject<IEntityTypeInitializerRegistry>();

			var globalSignatureTypeInitializer = new GlobalSignatureTypeInitializer(signatureSetInitializer);

			entityTypeInitializerRegistry.RegisterEntityTypeInitializer(globalSignatureTypeInitializer);
			configurationRegistry.RegisterSignatureTypeConfiguration(new SignatureTypeConfiguration("Global", new[] { "globalEntity" }));
		}
	}
}