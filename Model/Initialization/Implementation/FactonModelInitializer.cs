// <copyright file="FactonModelInitializer.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Collections.Generic;
	using System.Linq;
	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	public class FactonModelInitializer : IFactonModelInitializer
	{
		private readonly IModelConfigurationRegistry configurationRegistry;

		private readonly IEntityTypeInitializerRegistry entityTypeInitializerRegistry;

		private readonly IDefaultOperationHandlerProvider defaultOperationHandlerProvider;

		private readonly IPublicEntityTypeInitializer publicEntityTypeInitializer;

		private readonly IEnumerable<IComplexDataType> complexDataTypes;

		public FactonModelInitializer(
			IModelConfigurationRegistry configurationRegistry,
			IEntityTypeInitializerRegistry entityTypeInitializerRegistry,
			IDefaultOperationHandlerProvider defaultOperationHandlerProvider,
			IPublicEntityTypeInitializer publicEntityTypeInitializer,
			IEnumerable<IComplexDataType> complexDataTypes)
		{
			this.configurationRegistry = configurationRegistry;
			this.entityTypeInitializerRegistry = entityTypeInitializerRegistry;
			this.defaultOperationHandlerProvider = defaultOperationHandlerProvider;
			this.publicEntityTypeInitializer = publicEntityTypeInitializer;
			this.complexDataTypes = complexDataTypes;
		}

		public void Initialize(IBindableModelBuilder modelBuilder)
		{
			modelBuilder
				.WithNamespace("facton")
				.WithContainerName("container")
				.WithComplexDataTypes(this.complexDataTypes);

			var publicEntityTypeBuilder = this.publicEntityTypeInitializer.Initialize(modelBuilder);
			this.InitializeCoreTypes(modelBuilder, publicEntityTypeBuilder);
			this.InitializeSignatureTypes(modelBuilder, publicEntityTypeBuilder);

			this.RegisterGlobalOperationHandlers(modelBuilder);
			this.PerformPostProcessing(modelBuilder, publicEntityTypeBuilder);
		}

		private void InitializeCoreTypes(IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityTypeBuilder)
		{
			this.entityTypeInitializerRegistry.GetAll()
				.OfType<ICoreTypeInitializer>()
				.ForEach(i => i.Initialize(modelBuilder, publicEntityTypeBuilder));
		}

		private void InitializeSignatureTypes(IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityTypeBuilder)
		{
			// create one entity type per every registered FACTON signature type
			foreach (var signatureType in this.configurationRegistry.RegisteredSignatureTypeConfigurations)
			{
				var initializer =
					this.entityTypeInitializerRegistry.GetAll()
						.OfType<ISignatureTypeInitializer>()
						.FirstOrDefault(i => i.CanHandleSignatureType(signatureType));

				initializer?.Initialize(signatureType, modelBuilder, publicEntityTypeBuilder);
			}
		}

		private void PerformPostProcessing(IModelBuilder modelBuilder, IEntityTypeBuilder publicEntityTypeBuilder)
		{
			this.entityTypeInitializerRegistry.GetAll()
				.OfType<IPostProcessor>()
				.ForEach(p => p.PerformPostProcessing(modelBuilder, publicEntityTypeBuilder));
		}

		private void RegisterGlobalOperationHandlers(IModelBuilder modelBuilder)
		{
			modelBuilder
				.WithOperationHandler(this.defaultOperationHandlerProvider.ProvideDefaultGetHandler())
				.WithOperationHandler(this.defaultOperationHandlerProvider.ProvideDefaultPatchHandler())
				.WithOperationHandler(this.defaultOperationHandlerProvider.ProvideDefaultPostHandler());
		}
	}
}