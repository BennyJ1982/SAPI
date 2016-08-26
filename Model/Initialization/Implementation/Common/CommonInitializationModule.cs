// <copyright file="CommonInitializationModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Data.Entity.Design.PluralizationServices;
	using System.Globalization;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;
	using Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers;

	/// <summary>
	/// Module that provides common elements for initializing the FACTON Service API ODATA model
	/// </summary>
	public class CommonInitializationModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var metadataService = typeRegistry.GetObject<IMetadataService>();

			var queryBuilderFactory = typeRegistry.GetObject<IQueryBuilderFactory>();
			var entityCreator = typeRegistry.GetObject<IEntityCreator>();
			var entityReader = typeRegistry.GetObject<IEntityReader>();
			var entityUpdater = typeRegistry.GetObject<IEntityUpdater>();
			var queryService = typeRegistry.GetObject<IFactonQueryService>();
			var dtoBuilderFactory = typeRegistry.GetObject<IODataEntityDtoBuilderFactory>();

			var defaultOperationHandlerProvider = new DefaultOperationHandlerProvider(entityReader, entityCreator, entityUpdater, dtoBuilderFactory);
			var pluralizationService =
				new EntityFrameworkPluralizationServiceWrapper(PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-US")));
			var signatureSetInitializer = new SignatureEntitySetInitializer(metadataService, pluralizationService, queryService, queryBuilderFactory);

			typeRegistry.RegisterInstance<IDefaultOperationHandlerProvider>(defaultOperationHandlerProvider);
			typeRegistry.RegisterInstance<IPropertyService>(new PropertyService(metadataService));
			typeRegistry.RegisterInstance<ISignatureEntitySetInitializer>(signatureSetInitializer);
		}
	}
}