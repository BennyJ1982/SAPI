// <copyright file="ODataMappingModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization;
	using Facton.ServiceApi.Domain.Repository;

	/// <summary>
	/// Module that provides the ODataMappingService which can be used to initialize the mapping.
	/// </summary>
	public class ODataMappingModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var operationContextFactory = typeRegistry.GetObject<IOperationContextFactory>();
			var modelBuilderFactory = typeRegistry.GetObject<IBindableModelBuilderFactory>();
			var modelInitializer = typeRegistry.GetObject<IFactonModelInitializer>();

			var repositoryFactory = new RepositoryFactory(modelInitializer, operationContextFactory, modelBuilderFactory);
			var mappingService = new ODataMappingService(repositoryFactory);

			// provide mapping service
			typeRegistry.RegisterInstance<IODataMappingService>(mappingService);
		}
	}
}