// <copyright file="VersionsInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Versions
{
	using Facton.Domain.Entities.Versions;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Module that provides the initializer for entities that support versions.
	/// </summary>
	public class VersionsInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var metadataService = typeRegistry.GetObject<IMetadataService>();
			var versionService = typeRegistry.GetObject<IReadOnlyVersionService>();
			var queryBuilderFactory = typeRegistry.GetObject<IQueryBuilderFactory>();
			var queryService = typeRegistry.GetObject<IFactonQueryService>();
			var propertyFactory = typeRegistry.GetObject<IStructuralPropertyFactory>();

			var signatureSetInitializer = typeRegistry.GetObject<ISignatureEntitySetInitializer>();
			var configurationRegistry = typeRegistry.GetObject<IModelConfigurationRegistry>();
			var entityTypeInitializerRegistry = typeRegistry.GetObject<IEntityTypeInitializerRegistry>();

			var versionInfoInitializer = new VersionInfoTypeInitializer(queryBuilderFactory, metadataService, propertyFactory, queryService);
			var versionSupportingSignatureTypeInitializer = new VersionSupportingSignatureTypeInitializer(
				signatureSetInitializer,
				versionInfoInitializer);

			entityTypeInitializerRegistry.RegisterEntityTypeInitializer(versionSupportingSignatureTypeInitializer);

			configurationRegistry.RegisterSignatureTypeConfiguration(new SignatureTypeConfiguration("VariantFolder", new[] { "variantFolder" }));
			configurationRegistry.RegisterSignatureTypeConfiguration(
				new SignatureTypeConfiguration("Calculation", new[] { "calculation", "variant" }));
		}
	}
}