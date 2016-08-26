// <copyright file="ModelInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Linq;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Module that provides the initializer for the FACTON Service API ODATA model
	/// </summary>
	public class ModelInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var dataTypeRegistry = typeRegistry.GetObject<IDataTypeRegistry>();
			var mappingLogger = typeRegistry.GetObject<IMappingLogger>();
			var structuralPropertyFactory = typeRegistry.GetObject<IStructuralPropertyFactory>();

			var defaultOperationHandlerProvider = typeRegistry.GetObject<IDefaultOperationHandlerProvider>();
			var configurationRegistry = typeRegistry.GetObject<IModelConfigurationRegistry>();
			var entityTypeInitializerRegistry = typeRegistry.GetObject<IEntityTypeInitializerRegistry>();
			var propertyService = typeRegistry.GetObject<IPropertyService>();

			var publicEntityTypeInitializer = new PublicEntityTypeInitializer(propertyService, structuralPropertyFactory, mappingLogger);

			var modelInitializer = new FactonModelInitializer(
				configurationRegistry,
				entityTypeInitializerRegistry,
				defaultOperationHandlerProvider,
				publicEntityTypeInitializer,
				dataTypeRegistry.GetAll().OfType<IComplexDataType>());

			typeRegistry.RegisterInstance<IFactonModelInitializer>(modelInitializer);
		}
	}
}