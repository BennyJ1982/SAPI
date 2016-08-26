// <copyright file="EntitiesModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities.Queries;
	using Facton.Infrastructure.Modularity;
	using Facton.Infrastructure.Queries;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Execution.Reading;
	using Facton.ServiceApi.Domain.Model.Entities.PropertyMapping;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	/// <summary>
	/// The module that provides entity related services for the service API ODATA Model.
	/// </summary>
	public class EntitiesModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var modelBuilderFactory = typeRegistry.GetObject<IModelBuilderFactory>();
			var dataTypeRegistry = typeRegistry.GetObject<IDataTypeRegistry>();
			var modelItemNamingService = typeRegistry.GetObject<IModelItemNamingService>();
			var odataObjectFactory = typeRegistry.GetObject<IODataObjectFactory>();

			var structuralPropertyBinder = new StructuralPropertyBinder();
			var navigationPropertyBinder = new NavigationPropertyBinder();

			var entityReader = new EntityReader();
			var referenceParser = new ReferenceParser(entityReader);
			var uncontainedNavigationPropertyParser = new UncontainedNavigationPropertyParser(referenceParser);
			var dependencyResolver = new DependencyResolver(uncontainedNavigationPropertyParser);

			var entityCreator = new EntityCreator(
				structuralPropertyBinder,
				navigationPropertyBinder,
				uncontainedNavigationPropertyParser,
				dependencyResolver);

			var entityUpdater = new EntityUpdater(structuralPropertyBinder, navigationPropertyBinder, uncontainedNavigationPropertyParser);

			uncontainedNavigationPropertyParser.SetUncontainedEntitiesFactory(entityCreator.CreateInUncontainedNavigationProperty);

			typeRegistry.RegisterInstance<IBindableModelBuilderFactory>(new BindableModelBuilderFactory(modelBuilderFactory));
			typeRegistry.RegisterInstance<IEntityCreator>(entityCreator);
			typeRegistry.RegisterInstance<IEntityReader>(entityReader);
			typeRegistry.RegisterInstance<IEntityUpdater>(entityUpdater);
			typeRegistry.RegisterInstance<IFactonQueryService>(CreateQueryService(typeRegistry));
			typeRegistry.RegisterInstance<IStructuralPropertyFactory>(new StructuralPropertyFactory(dataTypeRegistry, modelItemNamingService));
			typeRegistry.RegisterInstance<IODataEntityDtoBuilderFactory>(new ODataEntityDtoBuilderFactory(odataObjectFactory));
		}

		private static IFactonQueryService CreateQueryService(IContext typeRegistry)
		{
			var entityQueryService = typeRegistry.GetObject<IEntityQueryService>();
			var queryService = typeRegistry.GetObject<IQueryService>();
			var mappingLogger = typeRegistry.GetObject<IMappingLogger>();

			return new FactonQueryService(queryService, entityQueryService, mappingLogger);
		}
	}
}