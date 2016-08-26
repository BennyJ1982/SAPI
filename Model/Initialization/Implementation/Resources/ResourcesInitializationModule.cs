// <copyright file="ResourcesInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Resources
{
	using System;
	using Facton.Domain.Resources;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Module that provides the initializer for Resources
	/// </summary>
	public class ResourcesInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var resourceService = typeRegistry.GetObject<IResourceService>();

			var structuralPropertyFactory = typeRegistry.GetObject<IStructuralPropertyFactory>();
			var signatureSetInitializer = typeRegistry.GetObject<ISignatureEntitySetInitializer>();
			var configurationRegistry = typeRegistry.GetObject<IModelConfigurationRegistry>();
			var entityTypeInitializerRegistry = typeRegistry.GetObject<IEntityTypeInitializerRegistry>();

			IStructuralProperty quantityTypeProperty;
			if (!structuralPropertyFactory.TryCreate(FactonModelKeywords.QuantityTypePropertyName, GetQuantityType, false, out quantityTypeProperty))
			{
				throw new InvalidOperationException("Could not map quantity type property.");
			}

			var resourceSignatureTypeInitializer = new ResourceSignatureTypeInitializer(
				signatureSetInitializer,
				resourceService,
				quantityTypeProperty);

			entityTypeInitializerRegistry.RegisterEntityTypeInitializer(resourceSignatureTypeInitializer);
			configurationRegistry.RegisterSignatureTypeConfiguration(new SignatureTypeConfiguration("Resource", new[] { "resource" }));
		}

		private static string GetQuantityType(IEntity entity)
		{
			return entity.As<IResource>().QuantityType.Name;
		}
	}
}