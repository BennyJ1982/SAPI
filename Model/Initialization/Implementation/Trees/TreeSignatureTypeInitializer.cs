namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using System;
	using System.Linq;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	using Microsoft.OData.Edm;

	/// <summary>
	/// Initializer for tree signature types
	/// </summary>
	public class TreeSignatureTypeInitializer : ISignatureTypeInitializer
	{
		private readonly ISignatureEntitySetInitializer signatureSetInitializer;

		private readonly IPropertyService propertyService;

		private readonly string signatureType;

		private readonly string space;

		public TreeSignatureTypeInitializer(
			ISignatureEntitySetInitializer signatureSetInitializer, 
			IPropertyService propertyService, 
			string signatureType, 
			string space)
		{
			this.signatureSetInitializer = signatureSetInitializer;
			this.propertyService = propertyService;
			this.signatureType = signatureType;
			this.space = space;
		}

		public bool CanHandleSignatureType(SignatureTypeConfiguration config)
		{
			return config.SignatureType == this.signatureType;
		}

		public IEntityTypeBuilder Initialize(
			SignatureTypeConfiguration config,
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder publicEntityType)
		{
			var dataTreeTypeBuilder = modelBuilder.CreateEntityTypeBuilder(config.SignatureType).WithParentType(publicEntityType);
			var parentProperty =
				dataTreeTypeBuilder.CreateUncontainedNavigationProperty(dataTreeTypeBuilder, FactonModelKeywords.TreeNodeParentNavigationPropertyName)
					.WithMultiplicity(EdmMultiplicity.Many, EdmMultiplicity.One)
					.WithSupportedOperations(Operation.Get);

			modelBuilder.WithBinding(parentProperty, new TreeNodeParentNavigationPropertyBinding());

			foreach (var signature in this.signatureSetInitializer.GetRelevantSignaturesByType(config.SignatureType))
			{
				var setBuilder =
					this.signatureSetInitializer.InitializeSet(
						modelBuilder, dataTreeTypeBuilder, signature, config.RelevantEntityTypes, this.space)
						.WithUncontainedNavigationPropertySelfTarget(parentProperty);

				IEntitySetBinding existingBinding;
				if (!modelBuilder.TryGetBinding(setBuilder, out existingBinding))
				{
					throw new InvalidOperationException("No existing binding found for tree node entity set.");
				}

				modelBuilder
					.WithBinding(setBuilder, new TreeNodeEntitySetBinding(existingBinding))
					.WithDependency(setBuilder, parentProperty)
					.WithOptionalDependency(setBuilder, publicEntityType.KeyProperties.First());

				this.AddTreePropertiesToPublicEntityType(signature, modelBuilder, dataTreeTypeBuilder, publicEntityType);
			}

			return dataTreeTypeBuilder;
		}

	
		private void AddTreePropertiesToPublicEntityType(
			ISignature treeSignature,
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder dataTreeType,
			IEntityTypeBuilder publicEntityType)
		{
			foreach (var property in this.propertyService.GetSelectionTreePropertiesByTreeSignature(treeSignature))
			{
				modelBuilder.WithBinding(
					publicEntityType.CreateUncontainedNavigationProperty(dataTreeType, property)
						.WithMultiplicity(EdmMultiplicity.One, EdmMultiplicity.One)
						.WithSupportedOperations(Operation.Get),
					new SelectionTreeValueNavigationPropertyBinding(property));
			}

			foreach (var property in this.propertyService.GetSelectionTreeValueListPropertiesByTreeSignature(treeSignature))
			{
				modelBuilder.WithBinding(
					publicEntityType.CreateUncontainedNavigationProperty(dataTreeType, property)
						.WithMultiplicity(EdmMultiplicity.One, EdmMultiplicity.Many)
						.WithSupportedOperations(Operation.Get),
					new SelectionTreeValueListValueNavigationPropertyBinding(property));
			}
		}
	}
}
