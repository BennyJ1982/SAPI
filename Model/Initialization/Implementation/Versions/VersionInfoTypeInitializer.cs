namespace Facton.ServiceApi.Domain.Model.Initialization.Versions
{
	using System;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	using Microsoft.OData.Edm;

	public class VersionInfoTypeInitializer : IVersionInfoTypeInitializer
	{
		private readonly IQueryBuilderFactory queryBuilderFactory;

		private readonly IMetadataService metadataService;

		private readonly IStructuralPropertyFactory structuralPropertyFactory;

		private readonly IFactonQueryService factonQueryService;

		public VersionInfoTypeInitializer(
			IQueryBuilderFactory queryBuilderFactory,
			IMetadataService metadataService,
			IStructuralPropertyFactory structuralPropertyFactory,
			IFactonQueryService factonQueryService)
		{
			this.queryBuilderFactory = queryBuilderFactory;
			this.metadataService = metadataService;
			this.structuralPropertyFactory = structuralPropertyFactory;
			this.factonQueryService = factonQueryService;
		}

		public IEntityTypeBuilder GetOrCreateVersionInfoType(IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityTypeBuilder)
		{
			IEntityTypeBuilder versionInfoTypeBuilder;
			if (!modelBuilder.TryGetEntityTypeBuilder(FactonModelKeywords.VersionInfoTypeName, out versionInfoTypeBuilder))
			{
				versionInfoTypeBuilder = modelBuilder.CreateEntityTypeBuilder(FactonModelKeywords.VersionInfoTypeName);
				IStructuralProperty versionNumberProperty;
				if (!this.structuralPropertyFactory.TryCreate(
						this.metadataService.GetPropertyByName("_VersionNumber"),
						"VersionNumber",
						false,
						out versionNumberProperty))
				{
					throw new InvalidOperationException("Cannot map version number property.");
				}

				versionInfoTypeBuilder.WithParentType(publicEntityTypeBuilder).WithStructuralProperty(versionNumberProperty);
				RegisterVersionedEntityNavigationProperty(modelBuilder, versionInfoTypeBuilder, publicEntityTypeBuilder);
			}

			return versionInfoTypeBuilder;
		}

		public void InitializeVersionsNavigationProperty(IBindableModelBuilder modelBuilder, IEntityTypeBuilder sourceTypeBuilder, IEntityTypeBuilder versionInfoTypeBuilder)
		{
			modelBuilder.WithBinding(
				sourceTypeBuilder.CreateContainedNavigationProperty(versionInfoTypeBuilder, FactonModelKeywords.VersionInfoNavigationPropertyName)
					.WithMultiplicity(EdmMultiplicity.One, EdmMultiplicity.Many)
					.WithSupportedOperations(Operation.Get | Operation.Post),
				new VersionInfoNavigationPropertyBinding(this.factonQueryService, this.queryBuilderFactory));
		}

		private static void RegisterVersionedEntityNavigationProperty(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder versionInfoTypeBuilder,
			IEntityTypeBuilder publicEntityTypeBuilder)
		{
			modelBuilder.WithBinding(
				versionInfoTypeBuilder.CreateContainedNavigationProperty(
					publicEntityTypeBuilder,
					FactonModelKeywords.VersionedEntityNavigationPropertyName)
					.WithMultiplicity(EdmMultiplicity.One, EdmMultiplicity.One)
					.WithSupportedOperations(Operation.Get),
				new VersionedEntityNavigationPropertyBinding());
		}
	}
}
