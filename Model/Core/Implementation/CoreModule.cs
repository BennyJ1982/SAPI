// <copyright file="CoreModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.Builders.Navigation;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;

	/// <summary>
	/// Module that provides the <see cref="IModelBuilderFactory"/>.
	/// </summary>
	public class CoreModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var odataObjectFactory = new ODataObjectFactory();

			var pathService = new ODataPathService();
			var modelItemNamingService = new ModelItemNamingService();
			var navigationPropertyBuilderFactory = new NavigationPropertyBuilderFactory(modelItemNamingService);

			var modelBuilderFactory = new ModelBuilderFactory(pathService, navigationPropertyBuilderFactory);
			var queryBuilderFactory = new QueryBuilderFactory();
			var operationContextFactory = new OperationContextFactory(pathService);
			var dataTypeRegistry = new DataTypeRegistry(new DefaultDataTypeKeyLookupStrategy());

			typeRegistry.RegisterInstance<IModelBuilderFactory>(modelBuilderFactory);
			typeRegistry.RegisterInstance<IDataTypeRegistry>(dataTypeRegistry);
			typeRegistry.RegisterInstance<IODataObjectFactory>(odataObjectFactory);
			typeRegistry.RegisterInstance<IODataPathService>(pathService);
			typeRegistry.RegisterInstance<IOperationContextFactory>(operationContextFactory);
			typeRegistry.RegisterInstance<IModelItemNamingService>(modelItemNamingService);
			typeRegistry.RegisterInstance<IQueryBuilderFactory>(queryBuilderFactory);
		}
	}
}